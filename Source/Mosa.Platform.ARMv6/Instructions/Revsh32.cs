// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Revsh32 - Byte-Reverse Signed Halfword
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Revsh32 : ARMv6Instruction
	{
		public override int ID { get { return 646; } }

		internal Revsh32()
			: base(1, 3)
		{
		}
	}
}
