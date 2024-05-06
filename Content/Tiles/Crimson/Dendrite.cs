﻿using StarlightRiver.Content.Biomes;
using Terraria.DataStructures;
using Terraria.ID;

namespace StarlightRiver.Content.Tiles.Crimson
{
	internal class DendriteItem : QuickTileItem
	{
		public DendriteItem() : base("Dendrite", "Doyoyouuyuuhuyhh *BRRPT* *Anyuerism*", "Dendrite", 0, "StarlightRiver/Assets/Tiles/Crimson/") { }
	}

	internal class Dendrite : ModTile
	{
		public override string Texture => "StarlightRiver/Assets/Tiles/Crimson/" + Name;

		public override void Load()
		{
			GraymatterBiome.onDrawOverPerTile += DrawRealVersion;
		}

		private void DrawRealVersion(SpriteBatch spriteBatch, int x, int y)
		{
			var target = new Point16(x, y);
			Tile tile = Framing.GetTileSafely(target);

			if (tile.TileType == ModContent.TileType<Dendrite>())
			{
				Texture2D tex = Assets.Tiles.Crimson.DendriteReal.Value;
				spriteBatch.Draw(tex, target.ToVector2() * 16 - Main.screenPosition, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Color.White * 0.8f);
			}
		}

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			Main.tileMerge[Type][TileID.CrimsonGrass] = true;
			Main.tileMerge[TileID.CrimsonGrass][Type] = true;

			Main.tileMerge[Type][TileID.Crimstone] = true;
			Main.tileMerge[TileID.Crimstone][Type] = true;

			Main.tileMergeDirt[Type] = true;

			HitSound = Terraria.ID.SoundID.Tink;

			DustType = Terraria.ID.DustID.Blood;
			RegisterItemDrop(ModContent.ItemType<DendriteItem>());

			AddMapEntry(new Color(165, 180, 191));
		}
	}
}