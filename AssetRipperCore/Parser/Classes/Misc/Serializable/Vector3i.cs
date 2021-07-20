using AssetRipper.Converters.Project;
using AssetRipper.Layout.Classes.Misc.Serializable;
using AssetRipper.Parser.IO.Asset;
using AssetRipper.Parser.IO.Asset.Reader;
using AssetRipper.Parser.IO.Asset.Writer;
using AssetRipper.YAML;
using System;

namespace AssetRipper.Parser.Classes.Misc.Serializable
{
	public struct Vector3i : IAsset
	{
		public Vector3i(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static bool operator ==(Vector3i left, Vector3i right)
		{
			return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
		}

		public static bool operator !=(Vector3i left, Vector3i right)
		{
			return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
		}

		public int GetValueByMember(int member)
		{
			member %= 3;
			if (member == 0)
			{
				return X;
			}
			if (member == 1)
			{
				return Y;
			}
			return Z;
		}

		public int GetMemberByValue(int value)
		{
			if (X == value)
			{
				return 0;
			}
			if (Y == value)
			{
				return 1;
			}
			if (Z == value)
			{
				return 2;
			}
			throw new ArgumentException($"Member with value {value} wasn't found");
		}

		public bool ContainsValue(int value)
		{
			if (X == value || Y == value || Z == value)
			{
				return true;
			}
			return false;
		}

		public void Read(AssetReader reader)
		{
			X = reader.ReadInt32();
			Y = reader.ReadInt32();
			Z = reader.ReadInt32();
		}

		public void Write(AssetWriter writer)
		{
			writer.Write(X);
			writer.Write(Y);
			writer.Write(Z);
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			Vector3iLayout layout = container.ExportLayout.Serialized.Vector3i;
			node.Style = MappingStyle.Flow;
			node.Add(layout.XName, X);
			node.Add(layout.YName, Y);
			node.Add(layout.ZName, Z);
			return node;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(Vector3i))
			{
				return false;
			}
			return this == (Vector3i)obj;
		}

		public override int GetHashCode()
		{
			int hash = 193;
			unchecked
			{
				hash = hash + 787 * X.GetHashCode();
				hash = hash * 823 + Y.GetHashCode();
				hash = hash * 431 + Z.GetHashCode();
			}
			return hash;
		}

		public override string ToString()
		{
			return $"[{X}, {Y}, {Z}]";
		}

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
	}
}
