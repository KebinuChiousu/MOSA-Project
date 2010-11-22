﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

using MbUnit.Framework;

using Mosa.Runtime.Loader;
using Mosa.Runtime;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;

namespace Test.Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface class for MbUnit3 to run our testcases.
	/// </summary>
	public abstract class TestFixtureBase
	{

		/// <summary>
		/// Holds the type system
		/// </summary>
		ITypeSystem typeSystem;

		/// <summary>
		/// 
		/// </summary>
		private Assembly loadedAssembly;

		/// <summary>
		/// The filename of the assembly, which contains the test case.
		/// </summary>
		private string assembly = null;

		/// <summary>
		/// Flag, which determines if the compiler needs to run.
		/// </summary>
		private bool needCompile = true;

		/// <summary>
		/// An array of assembly references to include in the compilation.
		/// </summary>
		private string[] references;

		/// <summary>
		/// The source text of the test code to compile.
		/// </summary>
		private string codeSource;

		/// <summary>
		/// Holds the target language of this test runner.
		/// </summary>
		private string language;

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		/// <summary>
		/// Determines if unsafe code is allowed in the test.
		/// </summary>
		private bool unsafeCode;

		private static string tempDirectory;

		private static string TempDirectory
		{
			get
			{
				if (tempDirectory == null)
				{
					tempDirectory = Path.Combine(Path.GetTempPath(), "mosa");
					if (!Directory.Exists(tempDirectory))
					{
						Directory.CreateDirectory(tempDirectory);
					}
				}
				return tempDirectory;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestFixtureBase"/> class.
		/// </summary>
		public TestFixtureBase()
		{
			references = new string[0];
			language = "C#";
		}

		/// <summary>
		/// Gets or sets a value indicating whether the test needs to be compiled.
		/// </summary>
		/// <value><c>true</c> if a compilation is needed; otherwise, <c>false</c>.</value>
		protected bool NeedCompile
		{
			get { return needCompile; }
			set { needCompile = value; }
		}

		/// <summary>
		/// Gets or sets the references.
		/// </summary>
		/// <value>The references.</value>
		public string[] References
		{
			get { return references; }
			set
			{
				if (references != value)
				{
					references = value;
					needCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language
		{
			get { return language; }
			set
			{
				if (language != value)
				{
					language = value;
					NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets the code source.
		/// </summary>
		/// <value>The code source.</value>
		public string CodeSource
		{
			get { return codeSource; }
			set
			{
				if (codeSource != value)
				{
					codeSource = value;
					NeedCompile = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether unsafe code is used in the test.
		/// </summary>
		/// <value><c>true</c> if unsafe code is used in the test; otherwise, <c>false</c>.</value>
		public bool UnsafeCode
		{
			get { return unsafeCode; }
			set
			{
				if (unsafeCode != value)
				{
					unsafeCode = value;
					NeedCompile = true;
				}
			}
		}

		public T Run<T>(string type, string method, params object[] parameters)
		{
			CompileTestCodeIfNecessary();

			Type delegateType = LocateDelegateInCompiledAssembly(type, method);

			IntPtr address = FindTestMethod(String.Empty, type, method);

			T result = default(T);
			object tempResult = ExecuteTestMethod(delegateType, parameters, address);
			try
			{
				result = (T)tempResult;
			}
			catch (InvalidCastException)
			{
				Assert.Fail(@"Failed to convert result {0} of type {1} to type {2}.", tempResult, tempResult.GetType(), typeof(T));
			}

			return result;
		}

		private Type LocateDelegateInCompiledAssembly(string type, string method)
		{
			string delegatename = BuildDelegateName(type, method);

			Type delegatetype = GetDelegateType(delegatename);

			return delegatetype;
		}

		private Type GetDelegateType(string delegatename)
		{
			if (loadedAssembly == null)
			{
				loadedAssembly = Assembly.LoadFile(assembly);
			}

			return loadedAssembly.GetType(delegatename, false);
		}

		private static string BuildDelegateName(string type, string method)
		{
			return type + "+R_" + method;
		}

		private static object ExecuteTestMethod(Type delegateType, object[] parameters, IntPtr address)
		{
			// Create a delegate for the test method
			Delegate fn = Marshal.GetDelegateForFunctionPointer(
				address,
				delegateType
			);

			// Execute the test method
			return fn.DynamicInvoke(parameters);
		}

		private IntPtr FindTestMethod(string ns, string type, string method)
		{
			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(ns, type, method);
			IntPtr address = runtimeMethod.Address;
			return address;
		}

		protected void CompileTestCodeIfNecessary()
		{
			// Do we need to compile the code?
			if (needCompile)
			{
				CompileTestCode();

				needCompile = false;
			}
		}

		/// <summary>
		/// Finds a runtime method, which represents the requested method.
		/// </summary>
		/// <exception cref="MissingMethodException">The sought method is not found.</exception>
		/// <param name="ns">The namespace of the sought method.</param>
		/// <param name="type">The type, which contains the sought method.</param>
		/// <param name="method">The method to find.</param>
		/// <returns>An instance of <see cref="RuntimeMethod"/>.</returns>
		private RuntimeMethod FindMethod(string ns, string type, string method)
		{
			foreach (RuntimeType t in typeSystem.GetCompiledTypes())
			{
				if (t.Namespace != ns || t.Name != type)
					continue;
				foreach (RuntimeMethod m in t.Methods)
				{
					if (m.Name == method)
					{
						return m;
					}
				}
			}

			throw new MissingMethodException(ns + @"." + type, method);
		}

		protected void CompileTestCode()
		{
			if (loadedAssembly != null)
			{
				loadedAssembly = null;
			}

			assembly = RunCodeDomCompiler();

			Console.WriteLine("Executing MOSA compiler...");
			RunMosaCompiler(assembly);
		}

		private string RunCodeDomCompiler()
		{
			CodeDomProvider provider;
			Console.WriteLine("Executing {0} compiler...", Language);
			if (!providerCache.TryGetValue(language, out provider))
				provider = CodeDomProvider.CreateProvider(Language);
			if (provider == null)
				throw new NotSupportedException("The language '" + Language + "' is not supported on this machine.");

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(References, filename, false);
			parameters.CompilerOptions = "/optimize-";

			if (unsafeCode)
			{
				if (Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}
			parameters.GenerateInMemory = false;
			if (codeSource != null)
			{
				compileResults = provider.CompileAssemblyFromSource(parameters, codeSource);
			}
			else
				throw new NotSupportedException();

			if (compileResults.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Code compile errors:");
				foreach (CompilerError error in compileResults.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				throw new Exception(sb.ToString());
			}

			return compileResults.PathToAssembly;
		}

		private void RunMosaCompiler(string assemblyFile)
		{
			List<string> files = new List<string>();
			files.Add(assemblyFile);

			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			//assemblyLoader.InitializePrivatePaths(files);
			//assemblyLoader.AppendPrivatePath(typeof(global::Mosa.Vm.Runtime).Module.FullyQualifiedName);

			typeSystem = new DefaultTypeSystem(assemblyLoader);
			typeSystem.LoadModules(files);
			//typeSystem.ResolveModuleReference(typeof(global::Mosa.Runtime.Runtime).Module.FullyQualifiedName);

			TestCaseAssemblyCompiler.Compile(typeSystem, assemblyLoader);
		}

		protected string CreateMarshalAttribute(string prefix, string typeName)
		{
			string result = String.Empty;
			string marshalDirective = GetMarshalDirective(typeName);
			if (marshalDirective != null)
			{
				result = @"[" + prefix + marshalDirective + @"]";
			}

			return result;
		}

		protected string GetMarshalDirective(string typeName)
		{
			string marshalDirective = null;

			if (typeName == @"char")
			{
				marshalDirective = @"MarshalAs(UnmanagedType.U2)";
			}

			return marshalDirective;
		}

	}
}
