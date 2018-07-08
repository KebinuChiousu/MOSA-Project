// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// CompareFloatR8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CompareFloatR8 : BaseIRInstruction
	{
		public override int ID { get { return 20; } }

		public CompareFloatR8()
			: base(2, 1)
		{
		}

		public override BuiltInType ResultType { get { return BuiltInType.Boolean; } }
	}
}
