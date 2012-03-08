/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.AVR32
{

	/// <summary>
	/// This class provides a common base class for architecture
	/// specific operations.
	/// </summary>
	public class Architecture : BasicArchitecture
	{

		/// <summary>
		/// Holds the calling conversion
		/// </summary>
		private ICallingConvention callingConvention;

		/// <summary>
		/// Defines the register set of the target architecture.
		/// </summary>
		private static readonly Register[] Registers = new Register[]
		{
			//TODO
		};

		/// <summary>
		/// Specifies the architecture features to use in generated code.
		/// </summary>
		private ArchitectureFeatureFlags architectureFeatures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Architecture"/> class.
		/// </summary>
		/// <param name="architectureFeatures">The features this architecture supports.</param>
		private Architecture(ArchitectureFeatureFlags architectureFeatures)
		{
			this.architectureFeatures = architectureFeatures;
		}

		/// <summary>
		/// Retrieves the native integer size of the x64 platform.
		/// </summary>
		/// <value>This property always returns 64.</value>
		public override int NativeIntegerSize
		{
			get { return 64; }
		}

		/// <summary>
		/// Retrieves the register set of the x64 platform.
		/// </summary>
		public override Register[] RegisterSet
		{
			get { return Registers; }
		}

		/// <summary>
		/// Retrieves the stack frame register of the x64.
		/// </summary>
		public override Register StackFrameRegister
		{
			get
			{
				// TODO
				return null;
				// return GeneralPurposeRegister.EBP; 
			}
		}

		/// <summary>
		/// Factory method for the Architecture class.
		/// </summary>
		/// <returns>The created architecture instance.</returns>
		/// <param name="architectureFeatures">The features available in the architecture and code generation.</param>
		/// <remarks>
		/// This method creates an instance of an appropriate architecture class, which supports the specific
		/// architecture features.
		/// </remarks>
		public static IArchitecture CreateArchitecture(ArchitectureFeatureFlags architectureFeatures)
		{
			if (architectureFeatures == ArchitectureFeatureFlags.AutoDetect)
				architectureFeatures = ArchitectureFeatureFlags.AutoDetect; // FIXME

			return new Architecture(architectureFeatures);
		}

		/// <summary>
		/// Creates a new result operand of the requested type.
		/// </summary>
		/// <param name="signatureType">The type requested.</param>
		/// <param name="instructionLabel">The label of the instruction requesting the operand.</param>
		/// <param name="operandStackIndex">The stack index of the operand.</param>
		/// <returns>A new operand usable as a result operand.</returns>
		public override Operand CreateResultOperand(SigType signatureType, int instructionLabel, int operandStackIndex)
		{
			// TODO
			return null;
			//return new RegisterOperand(signatureType, GeneralPurposeRegister.EAX);
		}

		/// <summary>
		/// Extends the assembly compiler pipeline with AVR32 specific stages.
		/// </summary>
		/// <param name="assemblyCompilerPipeline">The assembly compiler pipeline to extend.</param>
		public override void ExtendAssemblyCompilerPipeline(CompilerPipeline assemblyCompilerPipeline)
		{
			//assemblyCompilerPipeline.InsertAfterFirst<IAssemblyCompilerStage>(
			//    new InterruptVectorStage()
			//);

			//assemblyCompilerPipeline.InsertAfterFirst<InterruptVectorStage>(
			//    new ExceptionVectorStage()
			//);

			//assemblyCompilerPipeline.InsertAfterLast<TypeLayoutStage>(
			//    new MethodTableBuilderStage()
			//);

		}

		/// <summary>
		/// Extends the method compiler pipeline with AVR32 specific stages.
		/// </summary>
		/// <param name="methodCompilerPipeline">The method compiler pipeline to extend.</param>
		public override void ExtendMethodCompilerPipeline(CompilerPipeline methodCompilerPipeline)
		{

			methodCompilerPipeline.InsertAfterLast<PlatformStubStage>(
				new IMethodCompilerStage[]
			    {
					//new LongOperandTransformationStage(),
					//new AddressModeConversionStage(),
			        new IRTransformationStage(),
					//new TweakTransformationStage(),
					//new MemToMemConversionStage(),
			    });

			//methodCompilerPipeline.InsertAfterLast<IBlockOrderStage>(
			//    new SimplePeepholeOptimizationStage()
			//);

			//methodCompilerPipeline.InsertAfterLast<CodeGenerationStage>(
			//    new ExceptionLayoutStage()
			//);
		}

		/// <summary>
		/// Retrieves a calling convention object for the requested calling convention.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="ICallingConvention"/>.
		/// </returns>
		public override ICallingConvention GetCallingConvention()
		{
			// TODO
			if (callingConvention == null)
				callingConvention = null; // new DefaultCallingConvention(this);

			return callingConvention;

		}

		/// <summary>
		/// Gets the type memory requirements.
		/// </summary>
		/// <param name="signatureType">The signature type.</param>
		/// <param name="memorySize">Receives the memory size of the type.</param>
		/// <param name="alignment">Receives alignment requirements of the type.</param>
		public override void GetTypeRequirements(SigType signatureType, out int memorySize, out int alignment)
		{
			if (signatureType == null)
				throw new ArgumentNullException("signatureType");

			switch (signatureType.Type)
			{
				case CilElementType.U1: memorySize = alignment = 4; break;
				case CilElementType.U2: memorySize = alignment = 4; break;
				case CilElementType.U4: memorySize = alignment = 4; break;
				case CilElementType.U8: memorySize = 8; alignment = 4; break;
				case CilElementType.I1: memorySize = alignment = 4; break;
				case CilElementType.I2: memorySize = alignment = 4; break;
				case CilElementType.I4: memorySize = alignment = 4; break;
				case CilElementType.I8: memorySize = 8; alignment = 4; break;
				case CilElementType.R4: memorySize = alignment = 4; break;
				case CilElementType.R8: memorySize = alignment = 8; break;
				case CilElementType.Boolean: memorySize = alignment = 4; break;
				case CilElementType.Char: memorySize = alignment = 4; break;

				// Platform specific
				case CilElementType.Ptr: memorySize = alignment = 4; break;
				case CilElementType.I: memorySize = alignment = 4; break;
				case CilElementType.U: memorySize = alignment = 4; break;
				case CilElementType.Object: memorySize = alignment = 4; break;
				case CilElementType.Class: memorySize = alignment = 4; break;
				case CilElementType.String: memorySize = alignment = 4; break;

				default: memorySize = alignment = 4; break;
			}
		}

		/// <summary>
		/// Gets the intrinsic instruction by type
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public override IIntrinsicMethod GetIntrinsicMethod(Type type)
		{
			// TODO
			return null;
			//return Intrinsic.Method.Get(type);
		}

		/// <summary>
		/// Gets the code emitter.
		/// </summary>
		/// <returns></returns>
		public override ICodeEmitter GetCodeEmitter()
		{
			// TODO
			return null;
			//return new MachineCodeEmitter();
		}
	}
}