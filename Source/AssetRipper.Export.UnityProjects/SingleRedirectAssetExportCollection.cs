﻿using AssetRipper.Assets;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.Export;
using AssetRipper.Assets.Metadata;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.SerializedFiles;
using System.Diagnostics;

namespace AssetRipper.Export.UnityProjects;

/// <summary>
/// A collection that redirects a single <paramref name="Asset"/> to a known <paramref name="Pointer"/>.
/// </summary>
/// <remarks>
/// This is a simpler and more efficient alternative to <see cref="RedirectExportCollection"/> when only a single asset is being redirected.
/// </remarks>
/// <param name="Asset">The asset being redirected.</param>
/// <param name="Pointer">The location of <paramref name="Asset"/>.</param>
public sealed record class SingleRedirectAssetExportCollection(IUnityObjectBase Asset, MetaPtr Pointer) : IExportCollection
{
	public SingleRedirectAssetExportCollection(IUnityObjectBase asset, long fileID, UnityGuid guid, AssetType type) : this(asset, new MetaPtr(fileID, guid, type))
	{
	}

	AssetCollection IExportCollection.File => Asset.Collection;

	TransferInstructionFlags IExportCollection.Flags => Asset.Collection.Flags;

	IEnumerable<IUnityObjectBase> IExportCollection.Assets => [Asset];

	public string Name => Asset.GetBestName();

	bool IExportCollection.Contains(IUnityObjectBase asset)
	{
		return ReferenceEquals(Asset, asset);
	}

	MetaPtr IExportCollection.CreateExportPointer(IExportContainer container, IUnityObjectBase asset, bool isLocal)
	{
		ThrowIfLocal(isLocal);
		ThrowIfNotAsset(asset);
		return Pointer;

		[StackTraceHidden]
		static void ThrowIfLocal(bool isLocal)
		{
			if (isLocal)
			{
				throw new NotSupportedException();
			}
		}
	}

	bool IExportCollection.Export(IExportContainer container, string projectDirectory)
	{
		return true; //successfully redirected
	}

	long IExportCollection.GetExportID(IExportContainer container, IUnityObjectBase asset)
	{
		ThrowIfNotAsset(asset);
		return Pointer.FileID;
	}

	[StackTraceHidden]
	private void ThrowIfNotAsset(IUnityObjectBase asset)
	{
		if (!ReferenceEquals(Asset, asset))
		{
			throw new ArgumentException($"The asset must be the same one referenced in this collection.", nameof(asset));
		}
	}
}