﻿using AssetRipper.Parser.Files.File.Version;
using AssetRipper.Parser.IO.Asset.Reader;

namespace AssetRipper.Parser.Classes.AnimatorController.Mask
{
	public struct HumanPoseMask : IAssetReadable
	{
		/// <summary>
		/// 5.2.0 and greater
		/// </summary>
		public static bool HasSecondWord(Version version) => version.IsGreaterEqual(5, 2);

		public void Read(AssetReader reader)
		{
			m_word0 = reader.ReadUInt32();
			m_word1 = reader.ReadUInt32();
			if (HasSecondWord(reader.Version))
			{
				m_word2 = reader.ReadUInt32();
			}
		}

		public uint m_word0;
		public uint m_word1;
		public uint m_word2;
	}
}
