﻿namespace System
{
	public class Object
	{
		private IntPtr methodTablePtr;
		private IntPtr syncBlock;

		public Object()
		{
		}

		public virtual int GetHashCode()
		{
			return 0;
		}

		public virtual string ToString()
		{
			return null;
		}

		public virtual bool Equals(object obj)
		{
			return true;
		}

		public virtual void Finalize()
		{
		}
	}
}

namespace System.Runtime.InteropServices
{

	public enum UnmanagedType
	{
		Bool = 2,
		I1 = 3,
		U1 = 4,
		I2 = 5,
		U2 = 6,
		I4 = 7,
		U4 = 8,
		I8 = 9,
		U8 = 10,
		R4 = 11,
		R8 = 12
	}

	public sealed class MarshalAsAttribute : Attribute
	{
		public MarshalAsAttribute(short unmanagedType)
		{
		}

		public MarshalAsAttribute(UnmanagedType unmanagedType)
		{
		}
	}
}

namespace System
{
	public class ValueType : Object
	{
	}

	public class Enum : ValueType
	{
	}

	public class Delegate : Object
	{
	}

	public struct SByte
	{
		public const sbyte MinValue = -128;
		public const sbyte MaxValue = 127;
		internal sbyte _value;

		public int CompareTo(SByte value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Byte
	{
		public const byte MinValue = 0;
		public const byte MaxValue = 255;

		internal byte _value;

		public int CompareTo(byte value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Int16
	{
		public const short MaxValue = 32767;
		public const short MinValue = -32768;

		internal short _value;

		public int CompareTo(short value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Int32
	{
		public const int MaxValue = 0x7fffffff;
		public const int MinValue = -2147483648;

		internal int _value;

		public int CompareTo(int value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Int64
	{
		public const long MaxValue = 0x7fffffffffffffff;
		public const long MinValue = -9223372036854775808;

		internal long _value;

		public int CompareTo(long value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct UInt16
	{
		public const ushort MaxValue = 0xffff;
		public const ushort MinValue = 0;

		internal ushort _value;

		public int CompareTo(ushort value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct UInt32
	{
		public const uint MaxValue = 0xffffffff;
		public const uint MinValue = 0;

		internal uint _value;

		public int CompareTo(uint value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct UInt64
	{
		public const ulong MaxValue = 0xffffffffffffffff;
		public const ulong MinValue = 0;

		internal ulong _value;

		public int CompareTo(ulong value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Single
	{
		public const float Epsilon = 1.4e-45f;
		public const float MaxValue = 3.40282346638528859e38f;
		public const float MinValue = -3.40282346638528859e38f;
		public const float NaN = 0.0f / 0.0f;
		public const float PositiveInfinity = 1.0f / 0.0f;
		public const float NegativeInfinity = -1.0f / 0.0f;

		internal float _value;

		public static bool IsNaN(double d)
		{
#pragma warning disable 1718
			return (d != d);
#pragma warning restore
		}

		public static bool IsNegativeInfinity(double d)
		{
			return (d < 0.0f && (d == NegativeInfinity || d == PositiveInfinity));
		}

		public static bool IsPositiveInfinity(double d)
		{
			return (d > 0.0f && (d == NegativeInfinity || d == PositiveInfinity));
		}

		public static bool IsInfinity(double d)
		{
			return (d == PositiveInfinity || d == NegativeInfinity);
		}

		public int CompareTo(double value)
		{
			if (IsPositiveInfinity(_value) && IsPositiveInfinity(value))
				return 0;
			if (IsNegativeInfinity(_value) && IsNegativeInfinity(value))
				return 0;

			if (IsNaN(value)) if (IsNaN(_value))
					return 0;
				else
					return 1;

			if (IsNaN(_value))
				if (IsNaN(value))
					return 0;
				else
					return -1;

			if (_value > value)
				return 1;
			else if (_value < value)
				return -1;
			else
				return 0;
		}
	}

	public struct Double
	{
		public const double Epsilon = 4.9406564584124650e-324;
		public const double MaxValue = 1.7976931348623157e308;
		public const double MinValue = -1.7976931348623157e308;
		public const double NaN = 0.0d / 0.0d;
		public const double NegativeInfinity = -1.0d / 0.0d;
		public const double PositiveInfinity = 1.0d / 0.0d;

		internal double _value;

		public static bool IsNaN(double d)
		{
#pragma warning disable 1718
			return (d != d);
#pragma warning restore
		}

		public static bool IsNegativeInfinity(double d)
		{
			return d == NegativeInfinity;
		}

		public static bool IsPositiveInfinity(double d)
		{
			return d == PositiveInfinity;
		}

		public static bool IsInfinity(double d)
		{
			return (d == PositiveInfinity || d == NegativeInfinity);
		}

		public int CompareTo(double value)
		{
			if (IsPositiveInfinity(_value))
				if (IsPositiveInfinity(value))
					return 0;
			if (IsNegativeInfinity(_value))
				if (IsNegativeInfinity(value))
					return 0;

			if (IsNaN(value)) if (IsNaN(_value))
					return 0;
				else
					return 1;

			if (IsNaN(_value))
				if (IsNaN(value))
					return 0;
				else
					return -1;

			if (_value > value)
				return 1;
			else if (_value < value)
				return -1;
			else
				return 0;
		}
	}

	public struct Char
	{
		public const char MaxValue = (char)0xffff;
		public const char MinValue = (char)0;

		internal char _value;

		public int CompareTo(char value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}
	}

	public struct Boolean
	{
		internal bool _value;

		public int CompareTo(bool value)
		{
			if (!_value)
				if (value)
					return -1;
			if (_value)
				if (!value)
					return 1;
			return 0;
		}
	}

	public struct IntPtr
	{
		private int value;
	}

	public struct UIntPtr
	{
		private uint value;
	}

	public struct Decimal
	{
	}

	public class String
	{
		private int length;
		private char first_char;

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		public unsafe char this[int index]
		{
			get
			{
				/*
				 * HACK: This is not GC safe. Once we have GCHandle and the other structures,
				 * we need to wrap this in fixed-Block.
				 * 
				 */

				char result;

				fixed (char* pChars = &this.first_char)
				{
					result = pChars[index];
				}

				return result;
			}
		}
	}

	public class MulticastDelegate : Delegate
	{
	}

	public class Array
	{
		private int length;

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void SetValue(object value, int index)
		{
			// TODO
		}

		/// <summary>
		/// 
		/// </summary>
		public object GetValue(int index)
		{
			// TODO
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
		{
			for (int s = 0, d = destinationIndex; s < length; s++, d++)
			{
				sourceArray.SetValue(destinationArray.GetValue(d), s + sourceIndex);
			}
		}
	}

	public class Exception
	{
	}

	public class Type
	{
	}

	public class Attribute
	{
	}

	public class ParamArrayAttribute : Attribute
	{
	}

	public struct RuntimeTypeHandle
	{
	}

	public struct RuntimeFieldHandle
	{
	}

	public interface IDisposable
	{
	}

	public struct Void
	{
	}

	namespace Runtime
	{
		namespace InteropServices
		{
			public class OutAttribute : Attribute
			{
			}
		}
	}

	namespace Reflection
	{
		public class DefaultMemberAttribute : Attribute
		{
			public DefaultMemberAttribute(string name)
			{
			}
		}
	}

	namespace Collections
	{
		public interface IEnumerable
		{
		}

		public interface IEnumerator
		{
		}
	}
}

namespace Mosa.Vm
{

	using System;

	public static class Runtime
	{
		public static unsafe void* AllocateObject(void* methodTable, uint classSize)
		{
			return null;
		}

		public static unsafe void* AllocateArray(void* methodTable, uint elementSize, uint elements)
		{
			return null;
		}

		public static object Box(ValueType valueType)
		{
			return null;
		}

		public static object Castclass(object obj, UIntPtr typeHandle)
		{
			return null;
		}

		public static bool IsInstanceOfType(object obj, UIntPtr typeHandle)
		{
			return false;
		}

		public unsafe static void Memcpy(byte* destination, byte* source, int count)
		{
		}

		public unsafe static void Memset(byte* destination, byte value, int count)
		{
		}

		public static void Rethrow()
		{
		}

		public static void Throw(object exception)
		{
		}

		public static void Unbox(object obj, ValueType valueType)
		{
		}
	}
}