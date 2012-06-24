/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class StackLayout
	{

		#region Data members

		private IArchitecture architecture;

		private int stackMemorySize = 0;

		private List<Operand> stack = new List<Operand>();

		private Operand[] parameters;

		//private StackSizeOperand stackSizeOperand;

		#endregion // Data members

		#region Properties

		/// <summary>
		/// Gets the size of the stack.
		/// </summary>
		/// <value>
		/// The size of the stack.
		/// </value>
		public int StackMemorySize { get { return stackMemorySize; } set { stackMemorySize = value; } }

		/// <summary>
		/// Gets the stack size operand.
		/// </summary>
		//public StackSizeOperand StackSizeOperand { get { return stackSizeOperand; } }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		public Operand[] Parameters { get { return parameters; } }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="StackLayout"/> class.
		/// </summary>
		/// <param name="architecture">The architecture.</param>
		/// <param name="parameters">The parameters.</param>
		public StackLayout(IArchitecture architecture, int parameters)
		{
			this.architecture = architecture;
			//this.stackSizeOperand = new StackSizeOperand(this);
			this.parameters = new Operand[parameters];
		}

		/// <summary>
		/// Allocates the stack operand.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateStackOperand(SigType type, bool localVariable)
		{
			int stackSlot = stack.Count + 1;

			Operand stackOperand;

			if (localVariable)
			{
				stackOperand = Operand.CreateLocalVariable(type, architecture.StackFrameRegister, stackSlot, null);
			}
			else
			{
				stackOperand = Operand.CreateStackLocalTemp(type, architecture.StackFrameRegister, stackSlot);
			}

			stack.Add(stackOperand);

			return stackOperand;
		}

		/// <summary>
		/// Gets the stack operand.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		public Operand GetStackOperand(int slot)
		{
			return stack[slot];
		}

		/// <summary>
		/// Sets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="param">The param.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand SetStackParameter(int index, RuntimeParameter param, SigType type)
		{
			parameters[index] = Operand.CreateParameter(type, architecture.StackFrameRegister, param);
			return parameters[index];
		}

		/// <summary>
		/// Gets the stack parameter.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public Operand GetStackParameter(int index)
		{
			return parameters[index];
		}

	}
}
