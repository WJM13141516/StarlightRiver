using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

using StarlightRiver.Core;

namespace StarlightRiver.Content.Tiles.JungleBloody
{
    public class VineJungleBloody : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = AssetDirectory.JungleBloodyTile + name;
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileCut[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorAlternateTiles = new int[]
            {
                TileType<GrassJungleBloody>(),
                TileType<VineJungleBloody>()
            };
            TileObjectData.addTile(Type);
            soundType = SoundID.Grass;
            dustType = 14;
            AddMapEntry(new Color(122, 38, 38));
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j + 1].active() && Main.tile[i, j - 9].type != Type)
            {
                if (Main.rand.Next(1) == 0)
                {
                    WorldGen.PlaceTile(i, j + 1, TileType<VineJungleBloody>(), true);
                }
            }
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (!Main.tile[i, j - 1].active())
            {
                WorldGen.KillTile(i, j, false, false, false);
                WorldGen.SquareTileFrame(i, j, true);
            }
            else if (Main.tile[i, j - 1].type != TileType<GrassJungleBloody>() && Main.tile[i, j - 1].type != TileType<VineJungleBloody>())
            {
                WorldGen.KillTile(i, j, false, false, false);
                WorldGen.SquareTileFrame(i, j, true);
            }
            return true;
        }
    }
}