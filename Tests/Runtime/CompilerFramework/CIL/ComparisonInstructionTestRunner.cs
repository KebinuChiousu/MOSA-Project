﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Text;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	public class ComparisonInstructionTestRunner<T> : TestFixtureBase
	{
		private const string TestClassName = @"ComparisonTestClass";

		public ComparisonInstructionTestRunner()
		{
			this.IncludeCeq = true;
			this.IncludeClt = true;
			this.IncludeCgt = true;
			this.IncludeCle = true;
			this.IncludeCge = true;
		}

		public string FirstType { get; set; }

		public bool IncludeCeq { get; set; }
		public bool IncludeClt { get; set; }
		public bool IncludeCgt { get; set; }
		public bool IncludeCle { get; set; }
		public bool IncludeCge { get; set; }

		private void SetTestCode()
		{
			string marshalFirstType = this.CreateMarshalAttribute(String.Empty, FirstType);

			StringBuilder codeBuilder = new StringBuilder();

			codeBuilder.Append(TestCodeHeader);

			if (this.IncludeCeq)
				codeBuilder.Append(TestCodeCeq);
			if (this.IncludeClt)
				codeBuilder.Append(TestCodeClt);
			if (this.IncludeCgt)
				codeBuilder.Append(TestCodeCgt);
			if (this.IncludeCle)
				codeBuilder.Append(TestCodeCle);
			if (this.IncludeCge)
				codeBuilder.Append(TestCodeCge);

			codeBuilder.Append(TestCodeFooter);

			codeBuilder.Append(Code.AllTestCode);

			codeBuilder
				.Replace(@"[[firsttype]]", FirstType)
				.Replace(@"[[secondtype]]", FirstType)
				.Replace(@"[[marshal-firsttype]]", marshalFirstType)
				.Replace(@"[[marshal-secondtype]]", marshalFirstType);

			CodeSource = codeBuilder.ToString();
		}

		public void Ceq(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CeqTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Clt(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CltTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cgt(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CgtTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cle(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CleTest", first, second);
			Assert.AreEqual(expected, result);
		}

		public void Cge(bool expected, T first, T second)
		{
			this.EnsureCodeSourceIsSet();
			bool result = this.Run<bool>(TestClassName, @"CgeTest", first, second);
			Assert.AreEqual(expected, result);
		}

		private void EnsureCodeSourceIsSet()
		{
			if (CodeSource == null)
			{
				this.SetTestCode();
			}
		}

		private const string TestCodeHeader = @"
			using System.Runtime.InteropServices;

			public static class ComparisonTestClass
			{
		";

		private const string TestCodeCeq = @"
				public delegate bool R_CeqTest([[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool CeqTest([[firsttype]] first, [[secondtype]] second)
				{
					return (first == second);
				}
			";

		private const string TestCodeClt = @"
				public delegate bool R_CltTest([[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool CltTest([[firsttype]] first, [[secondtype]] second)
				{
					return (first < second);
				}
			";

		private const string TestCodeCgt = @"
				public delegate bool R_CgtTest([[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool CgtTest([[firsttype]] first, [[secondtype]] second)
				{
					return (first > second);
				}
			";

		private const string TestCodeCle = @"
				public delegate bool R_CleTest([[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool CleTest([[firsttype]] first, [[secondtype]] second)
				{
					return (first <= second);
				}
			";

		private const string TestCodeCge = @"
				public delegate bool R_CgeTest([[marshal-firsttype]][[firsttype]] first, [[marshal-secondtype]][[secondtype]] second);

				public static bool CgeTest([[firsttype]] first, [[secondtype]] second)
				{
					return (first >= second);
				}
			";

		private const string TestCodeFooter = @"
			}
		";
	}
}
