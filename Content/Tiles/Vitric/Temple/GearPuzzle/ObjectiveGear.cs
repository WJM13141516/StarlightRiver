using StarlightRiver.Core.Systems;
using StarlightRiver.Core.Systems.DummyTileSystem;

namespace StarlightRiver.Content.Tiles.Vitric.Temple.GearPuzzle
{
	class ObjectiveGear : GearTile
	{
		public static readonly Asset<Texture2D> texture_AssetDirectory_VitricTile___CeramicGearLarge = ModContent.Request<Texture2D>(AssetDirectory.VitricTile + "CeramicGearLarge");
		public static readonly Asset<Texture2D> texture_AssetDirectory_VitricTile___CeramicGearMid = ModContent.Request<Texture2D>(AssetDirectory.VitricTile + "CeramicGearMid");
		public static readonly Asset<Texture2D> texture_AssetDirectory_VitricTile___CeramicGearSmall = ModContent.Request<Texture2D>(AssetDirectory.VitricTile + "CeramicGearSmall");
		public static readonly Asset<Texture2D> texture_AssetDirectory_Invisible = ModContent.Request<Texture2D>(AssetDirectory.Invisible);
		public static readonly Asset<Texture2D> texture_AssetDirectory_VitricTile___GearPeg = ModContent.Request<Texture2D>(AssetDirectory.VitricTile + "GearPeg");
		public override int DummyType => DummySystem.DummyType<ObjectiveGearDummy>();

		public override bool RightClick(int i, int j)
		{
			var dummy = Dummy(i, j) as GearTileDummy;

			if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.DebugStick>())
			{
				dummy.GearSize++;
				return true;
			}

			return true;
		}

		public override void OnEngage(GearTileEntity entity)
		{
			GearPuzzleHandler.engagedObjectives++;
		}
	}

	class ObjectiveGearDummy : GearTileDummy
	{
		public ObjectiveGearDummy() : base(ModContent.TileType<ObjectiveGear>()) { }

		public override void PostDraw(Color lightColor)
		{
			Texture2D pegTex = texture_AssetDirectory_VitricTile___GearPeg.Value;
			Main.spriteBatch.Draw(pegTex, Center - Main.screenPosition, null, lightColor, 0, pegTex.Size() / 2, 1, 0, 0);

			Texture2D tex = GearSize switch
			{
				0 => texture_AssetDirectory_Invisible.Value,
				1 => texture_AssetDirectory_VitricTile___CeramicGearSmall.Value,
				2 => texture_AssetDirectory_VitricTile___CeramicGearMid.Value,
				3 => texture_AssetDirectory_VitricTile___CeramicGearLarge.Value,
				_ => texture_AssetDirectory_VitricTile___CeramicGearSmall.Value,
			};

			Main.spriteBatch.Draw(tex, Center - Main.screenPosition, null, lightColor, Rotation, tex.Size() / 2, 1, 0, 0);
		}
	}

	[SLRDebug]
	class ObjectiveGearItem : QuickTileItem
	{
		public ObjectiveGearItem() : base("Gear puzzle Point", "{{Debug}} Item", "ObjectiveGear", 8, AssetDirectory.VitricTile + "GearPeg", true) { }
	}
}