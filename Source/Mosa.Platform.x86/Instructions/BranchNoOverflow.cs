// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// BranchNoOverflow
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class BranchNoOverflow : X86Instruction
	{
		public override int ID { get { return 336; } }

		internal BranchNoOverflow()
			: base(0, 0)
		{
		}

		public override string AlternativeName { get { return "JNO"; } }

		public static readonly byte[] opcode = new byte[] { 0x0F, 0x81 };

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		public override bool IsOverflowFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X86.BranchOverflow;
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == 0);
			System.Diagnostics.Debug.Assert(node.OperandCount == 0);
			System.Diagnostics.Debug.Assert(node.BranchTargets.Count >= 1);
			System.Diagnostics.Debug.Assert(node.BranchTargets[0] != null);

			emitter.Write(opcode);
			(emitter as X86CodeEmitter).EmitRelativeBranchTarget(node.BranchTargets[0].Label);
		}
	}
}
