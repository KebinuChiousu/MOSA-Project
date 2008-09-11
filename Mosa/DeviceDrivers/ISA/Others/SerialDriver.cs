﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.Kernel;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x03F8, PortRange = 8, IRQ = 4, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[ISADeviceSignature(AutoLoad = false, BasePort = 0x02F8, PortRange = 8, IRQ = 3, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[ISADeviceSignature(AutoLoad = false, BasePort = 0x03E8, PortRange = 8, IRQ = 4, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[ISADeviceSignature(AutoLoad = false, BasePort = 0x02E8, PortRange = 8, IRQ = 3, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class SerialDriver : ISAHardwareDevice, IDevice, IHardwareDevice, ISerialDevice
	{
		protected IReadOnlyIOPort rbrBase; // Receive Buffer Register (read only)		
		protected IWriteOnlyIOPort thrBase; // Transmitter Holding Register (write only)		
		protected IReadWriteIOPort ierBase; // Interrupt Enable Register		
		protected IReadWriteIOPort dllBase; // Divisor Latch (LSB and MSB)
		protected IReadWriteIOPort dlmBase;
		protected IReadOnlyIOPort iirBase; // Interrupt Identification Register (read only)		
		protected IWriteOnlyIOPort fcrBase; // FIFO Control Register (write only, 16550+ only)		
		protected IReadWriteIOPort lcrBase; // Line Control Register		
		protected IReadWriteIOPort mcrBase; // Modem Control Register		
		protected IReadWriteIOPort lsrBase; // Line Status Register		
		protected IReadWriteIOPort msrBase; // Modem Status Register		
		protected IReadWriteIOPort scrBase; // Scratch Register (16450+ and some 8250s, special use with some boards)

		protected SpinLock spinLock;

		protected const ushort fifoSize = 256;
		protected byte[] fifoBuffer;
		protected uint fifoStart;
		protected uint fifoEnd;

		#region Flags

		[Flags]
		private enum IER : byte
		{
			DR = 0x01, // Data ready, it is generated if data waits to be read by the CPU.
			THRE = 0x02, // THR Empty, this interrupt tells the CPU to write characters to the THR.
			SI = 0x04, // Status interrupt. It informs the CPU of occurred transmission errors during reception.
			MSI = 0x08 // Modem status interrupt. It is triggered whenever one of the delta-bits is set (see MSR).
		}

		[Flags]
		private enum FCR : byte
		{
			Enabled = 0x01, // FIFO enable.
			CLR_RCVR = 0x02, // Clear receiver FIFO. This bit is self-clearing.
			CLR_XMIT = 0x04, // Clear transmitter FIFO. This bit is self-clearing.
			DMA = 0x08, // DMA mode
			// Receiver FIFO trigger level
			TL1 = 0x00,
			TL4 = 0x40,
			TL8 = 0x80,
			TL14 = 0xC0,
		}

		[Flags]
		private enum LCR : byte
		{
			// Word length
			CS5 = 0x00, // 5bits
			CS6 = 0x01, // 6bits
			CS7 = 0x02, // 7bits
			CS8 = 0x03, // 8bits
			// Stop bit
			ST1 = 0x00, // 1
			ST2 = 0x04, // 2
			// Parity
			PNO = 0x00, // None
			POD = 0x08, // Odd
			PEV = 0x18, // Even
			PMK = 0x28, // Mark 
			PSP = 0x38, // Space

			BRK = 0x40,
			DLAB = 0x80,
		}

		[Flags]
		private enum MCR : byte
		{
			DTR = 0x01,
			RTS = 0x02,
			OUT1 = 0x04,
			OUT2 = 0x08,
			LOOP = 0x10,
		}

		[Flags]
		private enum LSR : byte
		{
			DR = 0x01, // Data Ready. Reset by reading RBR (but only if the RX FIFO is empty, 16550+).
			OE = 0x02, // Overrun Error. Reset by reading LSR. Indicates loss of data.
			PE = 0x04, // Parity Error. Indicates transmission error. Reset by LSR.
			FE = 0x08, // Framing Error. Indicates missing stop bit. Reset by LSR.
			BI = 0x10, // Break Indicator. Set if RxD is 'space' for more than 1 word ('break'). Reset by reading LSR.
			THRE = 0x20, // Transmitter Holding Register Empty. Indicates that a new word can be written to THR. Reset by writing THR.
			TEMT = 0x40, // Transmitter Empty. Indicates that no transmission is running. Reset by reading LSR.
		}

		[Flags]
		private enum MSR : byte
		{
			DCTS = 0x01,
			DDSR = 0x02,
			DRI = 0x04,
			DDCD = 0x08,
			CTS = 0x10,
			DSR = 0x20,
			RI = 0x40,
			DCD = 0x80
		}

		#endregion

		public SerialDriver() { }

		public override bool Setup()
		{
			base.name = "COM_0x" + base.busResources.GetIOPort(0,0).Address.ToString("X");

			rbrBase = base.busResources.GetIOPort(0,0); // Receive Buffer Register (read only)
			thrBase = base.busResources.GetIOPort(0,0); // Transmitter Holding Register (write only)
			ierBase = base.busResources.GetIOPort(0,1); // Interrupt Enable Register
			dllBase = base.busResources.GetIOPort(0,0); // Divisor Latch (LSB and MSB)
			dlmBase = base.busResources.GetIOPort(0,1);
			iirBase = base.busResources.GetIOPort(0,2); // Interrupt Identification Register (read only)
			fcrBase = base.busResources.GetIOPort(0,2); // FIFO Control Register (write only, 16550+ only)
			lcrBase = base.busResources.GetIOPort(0,3); // Line Control Register
			mcrBase = base.busResources.GetIOPort(0,4); // Modem Control Register
			lsrBase = base.busResources.GetIOPort(0,5); // Line Status Register
			msrBase = base.busResources.GetIOPort(0,6); // Modem Status Register
			scrBase = base.busResources.GetIOPort(0,7); // Scratch Register (16450+ and some 8250s, special use with some boards)

			this.fifoBuffer = new byte[fifoSize];
			this.fifoStart = 0;
			this.fifoEnd = 0;

			return true;
		}

		public override bool Probe() { return true; }   // not implemented 

		public override bool Start()
		{
			///TODO: auto detect - otherwise just assume one is there
			///TODO: could use BIOS to help w/ detection; 0x0400-x0403 supply base address for COM1-4

			// Disable all UART interrupts
			ierBase.Write8(0x00);

			// Enable DLAB (set baud rate divisor)
			lcrBase.Write8((byte)LCR.DLAB);

			// Set Baud rate
			int baudRate = 115200;
			int divisor = 115200 / baudRate;
			dllBase.Write8((byte)(divisor & 0xFF));
			dlmBase.Write8((byte)(divisor >> 8 & 0xFF));

			// Reset DLAB, Set 8 bits, no parity, one stop bit
			lcrBase.Write8((byte)(LCR.CS8 | LCR.ST1 | LCR.PNO));

			// Enable FIFO, clear them, with 14-byte threshold
			fcrBase.Write8((byte)(FCR.Enabled | FCR.CLR_RCVR | FCR.CLR_XMIT | FCR.TL14));

			// IRQs enabled, RTS/DSR set
			mcrBase.Write8((byte)(MCR.DTR | MCR.RTS | MCR.OUT2));

			// Interrupt when data received
			ierBase.Write8((byte)IER.DR);

			return true;
		}

		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		public override bool OnInterrupt()
		{
			ReadSerial();
			return true;
		}

		protected void AddToFIFO(byte value)
		{
			uint next = fifoEnd + 1;

			if (next == fifoSize)
				next = 0;

			if (next == fifoStart)
				return; // out of room

			fifoBuffer[next] = value;
			fifoEnd = next;
		}

		protected byte GetFromFIFO()
		{
			if (fifoEnd == fifoStart)
				return 0;	// should not happen

			byte value = fifoBuffer[fifoStart];

			fifoStart++;

			if (fifoStart == fifoSize)
				fifoStart = 0;

			return value;
		}

		protected bool IsFIFODataAvailable()
		{
			return (fifoEnd != fifoStart);
		}

		protected bool IsFIFOFull()
		{
			if ((((fifoEnd + 1) == fifoSize) ? 0 : fifoEnd + 1) == fifoStart)
				return true;
			else
				return false;
		}

		protected bool CanTransmit()
		{
			return ((lsrBase.Read8() & (byte)LSR.THRE) != 0);
		}

		public void Write(byte ch)
		{
			try {
				spinLock.Enter();

				while (!CanTransmit())
					;

				thrBase.Write8(ch);
			}
			finally {
				spinLock.Exit();
			}
		}

		protected bool CanRead()
		{
			return ((lsrBase.Read8()) & (byte)LSR.DR) != 0;
		}

		protected void ReadSerial()
		{
			try {
				spinLock.Enter();

				if (!IsFIFOFull())
					while (CanRead())
						AddToFIFO(rbrBase.Read8());
			}
			finally {
				spinLock.Exit();
			}
		}

		public void DisableDataReceivedInterrupt()
		{
			IER ier = (IER)(ierBase.Read8());
			ier &= ~IER.DR;
			ierBase.Write8((byte)ier);
		}

		public void EnableDataReceivedInterrupt()
		{
			byte ier = ierBase.Read8();
			ier |= (byte)IER.DR;
			ierBase.Write8(ier);
		}

		public int ReadByte()
		{
			try {
				spinLock.Enter();

				if (!IsFIFODataAvailable())
					return -1;

				return GetFromFIFO();
			}
			finally {
				spinLock.Exit();
			}
		}
	}
}