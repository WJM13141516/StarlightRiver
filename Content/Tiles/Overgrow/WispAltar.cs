﻿using Microsoft.Xna.Framework;
using StarlightRiver.Core;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Tiles.Overgrow
{
	class WispAltarL : ModTile
    {
        public override string Texture => AssetDirectory.OvergrowTile + "WispAltarL";

        public override void SetDefaults() => QuickBlock.QuickSetFurniture(this, 6, 11, DustType<Dusts.GoldNoMovement>(), SoundID.Tink, false, new Color(200, 200, 200));
    }

    class WispAltarLItem : QuickTileItem
    {
        public override string Texture => AssetDirectory.Debug;

        public WispAltarLItem() : base("Wisp Altar L Placer", "Debug Item", TileType<WispAltarL>(), -1) { }

    }

    class WispAltarR : ModTile
    {
        public override string Texture => AssetDirectory.OvergrowTile + "WispAltarR";

        public override void SetDefaults() => QuickBlock.QuickSetFurniture(this, 6, 11, DustType<Dusts.GoldNoMovement>(), SoundID.Tink, false, new Color(200, 200, 200));
    }

    class WispAltarRItem : QuickTileItem
    {
        public override string Texture => AssetDirectory.Debug;

        public WispAltarRItem() : base("Wisp Altar R Placer", "Debug Item", TileType<WispAltarR>(), -1) { }
    }
}