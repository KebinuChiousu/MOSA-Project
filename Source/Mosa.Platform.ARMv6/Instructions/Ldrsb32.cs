// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldrsb32 - Load 8-bit signed byte
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Ldrsb32 : ARMv6Instruction
	{
		public override int ID { get { return 631; } }

		internal Ldrsb32()
			: base(1, 3)
		{
		}
	}
}
