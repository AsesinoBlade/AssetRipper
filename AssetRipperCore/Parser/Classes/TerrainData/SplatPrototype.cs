using AssetRipper.Converters.Classes.TerrainData;
using AssetRipper.Converters.Project;
using AssetRipper.Parser.Asset;
using AssetRipper.Parser.Classes.Misc;
using AssetRipper.Parser.Classes.Misc.Serializable;
using AssetRipper.Parser.Files.File.Version;
using AssetRipper.Parser.IO.Asset;
using AssetRipper.Parser.IO.Asset.Reader;
using AssetRipper.Parser.IO.Asset.Writer;
using AssetRipper.YAML;
using System.Collections.Generic;

namespace AssetRipper.Parser.Classes.TerrainData
{
	public struct SplatPrototype : IAsset, IDependent
	{
		public SplatPrototype(bool _) :
			this()
		{
			TileSize = Vector2f.One;
		}

		/// <summary>
		/// 4.0.0 and greater
		/// </summary>
		public static bool HasNormalMap(Version version) => version.IsGreaterEqual(4);
		/// <summary>
		/// 3.0.0 and greater
		/// </summary>
		public static bool HasTileOffset(Version version) => version.IsGreaterEqual(3);
		/// <summary>
		/// 5.0.0f1 and greater (unknown version)
		/// </summary>
		public static bool HasSpecularMetallic(Version version) => version.IsGreaterEqual(5, 0, 0, VersionType.Final);
		/// <summary>
		/// 5.0.0p1 and greater
		/// </summary>
		public static bool HasSmoothness(Version version) => version.IsGreaterEqual(5, 0, 0, VersionType.Patch);

		public TerrainLayer Convert(IExportContainer container)
		{
			return SplatPrototypeConverter.GenerateTerrainLayer(container, ref this);
		}

		public void Read(AssetReader reader)
		{
			Texture.Read(reader);
			if (HasNormalMap(reader.Version))
			{
				NormalMap.Read(reader);
			}
			TileSize.Read(reader);
			if (HasTileOffset(reader.Version))
			{
				TileOffset.Read(reader);
			}
			if (HasSpecularMetallic(reader.Version))
			{
				SpecularMetallic.Read(reader);
			}
			if (HasSmoothness(reader.Version))
			{
				Smoothness = reader.ReadSingle();
			}
		}

		public void Write(AssetWriter writer)
		{
			Texture.Write(writer);
			if (HasNormalMap(writer.Version))
			{
				NormalMap.Write(writer);
			}
			TileSize.Write(writer);
			if (HasTileOffset(writer.Version))
			{
				TileOffset.Write(writer);
			}
			if (HasSpecularMetallic(writer.Version))
			{
				SpecularMetallic.Write(writer);
			}
			if (HasSmoothness(writer.Version))
			{
				writer.Write(Smoothness);
			}
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(TextureName, Texture.ExportYAML(container));
			if (HasNormalMap(container.ExportVersion))
			{
				node.Add(NormalMapName, NormalMap.ExportYAML(container));
			}
			node.Add(TileSizeName, TileSize.ExportYAML(container));
			if (HasTileOffset(container.ExportVersion))
			{
				node.Add(TileOffsetName, TileOffset.ExportYAML(container));
			}
			if (HasSpecularMetallic(container.ExportVersion))
			{
				node.Add(SpecularMetallicName, SpecularMetallic.ExportYAML(container));
			}
			if (HasSmoothness(container.ExportVersion))
			{
				node.Add(SmoothnessName, Smoothness);
			}
			return node;
		}

		public IEnumerable<PPtr<Object.Object>> FetchDependencies(DependencyContext context)
		{
			yield return context.FetchDependency(Texture, TextureName);
			if (HasNormalMap(context.Version))
			{
				yield return context.FetchDependency(NormalMap, NormalMapName);
			}
		}

		public float Smoothness { get; set; }

		public const string TextureName = "texture";
		public const string NormalMapName = "normalMap";
		public const string TileSizeName = "tileSize";
		public const string TileOffsetName = "tileOffset";
		public const string SpecularMetallicName = "specularMetallic";
		public const string SmoothnessName = "smoothness";

		public PPtr<Texture2D.Texture2D> Texture;
		public PPtr<Texture2D.Texture2D> NormalMap;
		public Vector2f TileSize;
		public Vector2f TileOffset;
		public Vector4f SpecularMetallic;
	}
}
