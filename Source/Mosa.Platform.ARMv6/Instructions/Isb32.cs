// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Isb32 - Instruction Synchronization Barrier
	/// </summary>
	/// <seealso cref="Mosa.Platform.ARMv6.ARMv6Instruction" />
	public sealed class Isb32 : ARMv6Instruction
	{
		public override int ID { get { return 710; } }

		internal Isb32()
			: base(1, 3)
		{
		}
	}
}
