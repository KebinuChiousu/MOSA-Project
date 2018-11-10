// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions
{
	/// <summary>
	/// Movzx8To32
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
	public sealed class Movzx8To32 : X64Instruction
	{
		public override int ID { get { return 516; } }

		internal Movzx8To32()
			: base(1, 1)
		{
		}

		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			System.Diagnostics.Debug.Assert(node.ResultCount == DefaultResultCount);
			System.Diagnostics.Debug.Assert(node.OperandCount == DefaultOperandCount);

			//StaticEmitters.EmitMovzx8To32(node, emitter);
		}
	}
}
