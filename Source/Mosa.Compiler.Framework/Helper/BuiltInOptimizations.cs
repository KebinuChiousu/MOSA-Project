﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Helper
{
	public static class BuiltInOptimizations
	{
		public static Operand ConstantFoldingAndStrengthReductionInteger(InstructionNode node)
		{
			return ConstantFolding2Integer(node) ?? ConstantFolding1Integer(node) ?? StrengthReductionInteger(node);
		}

		public static Operand ConstantFolding1Integer(InstructionNode node)
		{
			if (node.OperandCount != 1 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;

			if (instruction == IRInstruction.GetLow64)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.GetHigh64)
			{
				return ConstantOperand.Create(result.Type, (uint)(op1.ConstantUnsignedLongInteger >> 32));
			}
			else if (instruction == IRInstruction.LogicalNot32)
			{
				return ConstantOperand.Create(result.Type, ~((uint)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.LogicalNot64)
			{
				return ConstantOperand.Create(result.Type, ~((ulong)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.Truncation64x32)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.SignExtend8x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x32((byte)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend16x32)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x32((ushort)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend8x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend8x64((byte)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend16x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend16x64((ushort)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.SignExtend32x64)
			{
				return ConstantOperand.Create(result.Type, SignExtend32x64((uint)op1.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.ZeroExtend8x32)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend16x32)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend8x64)
			{
				return ConstantOperand.Create(result.Type, (byte)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend16x64)
			{
				return ConstantOperand.Create(result.Type, (ushort)op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ZeroExtend32x64)
			{
				return ConstantOperand.Create(result.Type, (uint)op1.ConstantUnsignedLongInteger);
			}

			return null;
		}

		public static Operand ConstantFolding2Integer(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant || !node.Operand2.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (instruction == IRInstruction.Add32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Add64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.Sub32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Sub64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalAnd32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalAnd64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalOr32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalXor32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalXor64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.ArithShiftRight32)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ArithShiftRight64)
			{
				return ConstantOperand.Create(result.Type, (ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.ShiftRight32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftRight64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ShiftLeft32)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftLeft64)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.RemSigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, ((ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemSigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.RemUnsigned32 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, (op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemUnsigned64 && !op2.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.To64)
			{
				return ConstantOperand.Create(result.Type, op2.ConstantUnsignedLongInteger << 32 | op1.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
			{
				bool compareResult = true;

				switch (node.ConditionCode)
				{
					case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedLongInteger == op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedLongInteger != op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;

					case ConditionCode.UnsignedGreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedGreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedLessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;
					case ConditionCode.UnsignedLessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;

					// TODO: Add more
					default: return null;
				}

				return ConstantOperand.Create(result.Type, compareResult ? 1 : 0);
			}

			return null;
		}

		public static Operand StrengthReductionInteger(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && (op1 == op2))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftRight32) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 32)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight64) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 64)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op1.IsConstantOne)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1.IsConstantZero)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, 1);
			}
			else if ((node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64) && op2.IsConstantOne)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalOr32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return ConstantOperand.Create(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1 == op2)
			{
				return ConstantOperand.Create(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
			{
				var condition = node.ConditionCode;

				if (op1 == op2 && (condition == ConditionCode.Equal || condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual || condition == ConditionCode.UnsignedLessOrEqual || condition == ConditionCode.LessOrEqual))
				{
					return ConstantOperand.Create(result.Type, true ? 1 : 0);
				}
				else if (op1 == op2 && (condition == ConditionCode.NotEqual || condition == ConditionCode.GreaterThan || condition == ConditionCode.LessThan || condition == ConditionCode.UnsignedGreaterThan || condition == ConditionCode.UnsignedLessThan))
				{
					return ConstantOperand.Create(result.Type, false ? 1 : 0);
				}
			}

			return null;
		}

		#region Helpers

		private static uint SignExtend8x32(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFF00);
		}

		private static uint SignExtend16x32(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFF0000);
		}

		private static ulong SignExtend8x64(byte value)
		{
			return ((value & 0x80) == 0) ? value : (value | 0xFFFFFFFFFFFFFF00ul);
		}

		private static ulong SignExtend16x64(ushort value)
		{
			return ((value & 0x8000) == 0) ? value : (value | 0xFFFFFFFFFFFF0000ul);
		}

		private static ulong SignExtend32x64(uint value)
		{
			return ((value & 0x80000000) == 0) ? value : (value | 0xFFFFFFFF00000000ul);
		}

		#endregion Helpers
	}
}
