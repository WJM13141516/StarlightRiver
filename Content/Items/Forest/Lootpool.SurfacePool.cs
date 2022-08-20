using StarlightRiver.Core.Systems.ChestLootSystem;
using StarlightRiver.Helpers;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Forest
{
    public class SurfacePool : LootPool
    {
        public override void AddLoot()
        {
            AddItem(ModContent.ItemType<AcornSprout>(), ChestRegionFlags.Surface, default, 1, true, 1);
            AddItem(ModContent.ItemType<DustyAmulet>(), ChestRegionFlags.Surface, default, 1, true, 1);
            AddItem(ModContent.ItemType<OldWhetstone>(), ChestRegionFlags.Surface, default, 1, true, 1);
            AddItem(ModContent.ItemType<Trowel>(), ChestRegionFlags.Surface, default, 1, true, 1);
        }
    }
}