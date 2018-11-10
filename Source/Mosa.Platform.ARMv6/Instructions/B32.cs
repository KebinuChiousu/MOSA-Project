// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// B32 - Branch to target address
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class B32 : ARMv6Instruction
	{
		public override int ID { get { return 699; } }

		internal B32()
			: base(1, 3)
		{
		}
	}
}
