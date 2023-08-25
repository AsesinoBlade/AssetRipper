﻿using AssetRipper.Assets;
using AssetRipper.Numerics;
using AssetRipper.SourceGenerated.Classes.ClassID_320;
using AssetRipper.SourceGenerated.Classes.ClassID_4;
using AssetRipper.SourceGenerated.Extensions;
using AssetRipper.SourceGenerated.Subclasses.ExposedReferenceTable;
using AssetRipper.SourceGenerated.Subclasses.IntegerString;
using AssetRipper.SourceGenerated.Subclasses.NestedString;
using AssetRipper.SourceGenerated.Subclasses.PPtr_Object;
using System.Numerics;

namespace AssetRipper.Processing.Editor;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Rules for the methods in this class:
/// <list type="bullet">
/// <item>All methods must be static.</item>
/// <item>All public methods must return void and be named Convert.</item>
/// <item>Each public method must only take one parameter, and that parameter's type must inherit from <see cref="IUnityObjectBase"/>.</item>
/// <item>They must not resolve any PPtrs.</item>
/// </list>
/// </remarks>
internal static class EditorFormatConverterAsync
{
	public static void Convert(ITransform transform)
	{
		if (transform.Has_RootOrder_C4())
		{
			transform.RootOrder_C4 = transform.CalculateRootOrder();
		}
		if (transform.Has_LocalEulerAnglesHint_C4())
		{
			Vector3 eulerHints = new Quaternion(
				transform.LocalRotation_C4.X,
				transform.LocalRotation_C4.Y,
				transform.LocalRotation_C4.Z,
				transform.LocalRotation_C4.W).ToEulerAngle(true);
			transform.LocalEulerAnglesHint_C4.SetValues(eulerHints.X, eulerHints.Y, eulerHints.Z);
		}
	}

	public static void Convert(IPlayableDirector playableDirector)
	{
		if (playableDirector.Has_ExposedReferences_C320())
		{
			IExposedReferenceTable table = playableDirector.ExposedReferences_C320;
			table.References_Editor.Clear();
			if (table.Has_References_Release_AssetDictionary_NestedString_PPtr_Object_5_0_0())
			{
				table.References_Editor.Capacity = table.References_Release_AssetDictionary_NestedString_PPtr_Object_5_0_0.Count;
				foreach ((NestedString key, PPtr_Object_5_0_0 value) in table.References_Release_AssetDictionary_NestedString_PPtr_Object_5_0_0)
				{
					table.References_Editor.Add(key.Id, value);
				}
			}
			else
			{
				table.References_Editor.Capacity = table.References_Release_AssetDictionary_IntegerString_PPtr_Object_5_0_0.Count;
				foreach ((IntegerString key, PPtr_Object_5_0_0 value) in table.References_Release_AssetDictionary_IntegerString_PPtr_Object_5_0_0)
				{
					//The release keys are int, but the editor keys are string.
					//There may be a way to convert the keys, such as ReverseCrc32, but we don't know if they're Crc32 hashes or not.
					//For now, we'll just generate a descriptive string.
					string keyString = $"UnknownString_0x{key.Id:X}";
					table.References_Editor.Add(keyString, value);
				}
			}
		}
	}
}
