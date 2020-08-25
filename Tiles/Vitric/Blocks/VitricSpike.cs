﻿using Microsoft.Xna.Framework;
using StarlightRiver.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Tiles.Vitric.Blocks
{
    internal class VitricSpike : ModTile
    {
        public override void SetDefaults()
        {
            minPick = int.MaxValue;
            QuickBlock.QuickSet(this, 200, DustType<Dusts.Glass3>(), SoundID.Tink, new Color(95, 162, 138), -1);
            Main.tileMerge[Type][TileType<VitricSand>()] = true;
            Main.tileMerge[Type][TileType<VitricLargeCrystal>()] = true;
            Main.tileMerge[Type][TileType<VitricSmallCrystal>()] = true;
        }

        public override bool Dangersense(int i, int j, Player player) => true;

        public override void FloorVisuals(Player player)
        {
            player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " thought glass shards would be soft..."), 25, 0);
            player.velocity.Y -= 15;
        }
    }

    class VitricSpikeItem : QuickTileItem
    {
        public VitricSpikeItem() : base("Vitric Spikes", "Ouch!", TileType<VitricSpike>(), 0) { }
    }
}