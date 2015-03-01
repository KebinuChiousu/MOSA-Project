/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public interface IBlockOrderAnalysis
	{
		void PerformAnalysis(BasicBlocks basicBlocks);

		#region Properties

		IList<BasicBlock> NewBlockOrder { get; }

		int GetLoopDepth(BasicBlock block);

		int GetLoopIndex(BasicBlock block);

		#endregion Properties
	}
}
