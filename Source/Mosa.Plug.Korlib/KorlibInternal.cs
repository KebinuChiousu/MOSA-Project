﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Collections.Generic;

namespace Mosa.Plug.Korlib
{
	internal static class KorlibInternal
	{
		internal static List<RuntimeAssembly> Assemblies;

		internal static void Setup()
		{
			Assemblies = new List<RuntimeAssembly>();

			// Get AssemblyListTable and Assembly count
			var assemblyListTable = Intrinsic.GetAssemblyListTable();
			uint assemblyCount = Intrinsic.Load32(assemblyListTable);

			// Loop through and populate the array
			for (int i = 0; i < assemblyCount; i++)
			{
				// Get the pointer to the Assembly Metadata
				var ptr = Intrinsic.LoadPointer(assemblyListTable, IntPtr.Size + (IntPtr.Size * i));

				//Assemblies.Add(new RuntimeAssembly(ptr));
			}
		}
	}
}
