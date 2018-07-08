// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// BranchGreaterOrEqual
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
	public sealed class BranchGreaterOrEqual : X86Instruction
	{
		public override int ID { get { return 352; } }

		internal BranchGreaterOrEqual()
			: base(0, 0)
		{
		}

		public override string AlternativeName { get { return "JGE"; } }

		public static readonly byte[] opcode = new byte[] { 0x0F, 0x8D };

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		public override bool IsSignFlagUsed { get { return true; } }

		public override bool IsOverflowFlagUsed { get { return true; } }

		public override BaseInstruction GetOpposite()
		{
			return X86.BranchLessThan;
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
