// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// InvlpgConst
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class InvlpgConst : X86Instruction
	{
		public override int ID { get { return 238; } }

		internal InvlpgConst()
			: base(0, 1)
		{
		}

		public override bool HasUnspecifiedSideEffect { get { return true; } }

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 1);

			emitter.OpcodeEncoder.AppendByte(0x0F);
			emitter.OpcodeEncoder.AppendByte(0x01);
			emitter.OpcodeEncoder.Append2Bits(0b00);
			emitter.OpcodeEncoder.Append3Bits(0b010);
			emitter.OpcodeEncoder.Append3Bits(0b101);
			emitter.OpcodeEncoder.Append32BitImmediate(node.Operand1);
		}
	}
}