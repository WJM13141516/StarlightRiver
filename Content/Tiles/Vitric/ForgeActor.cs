﻿using StarlightRiver.Core.Systems.DummyTileSystem;
using StarlightRiver.Core.Systems.LightingSystem;
using System.Collections.Generic;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace StarlightRiver.Content.Tiles.Vitric
{
	class ForgeActor : DummyTile
	{
		public override int DummyType => ProjectileType<ForgeActorDummy>();

		public override string Texture => AssetDirectory.Invisible;

		public override void SetStaticDefaults()
		{
			this.QuickSetFurniture(1, 1, DustType<Dusts.Air>(), SoundID.Shatter, false, Color.Black);
		}
	}

	class ForgeActorDummy : Dummy
	{
		public ForgeActorDummy() : base(TileType<ForgeActor>(), 16, 16) { }

		public override void SafeSetDefaults()
		{
			Projectile.hide = true;
		}

		public override void Update()
		{
			if (Main.rand.NextBool(4))
			{
				Vector2 pos = Projectile.position - new Vector2(567, 400);

				Dust.NewDustPerfect(pos + new Vector2(175 + Main.rand.Next(-15, 15), 368), DustType<Dusts.Cinder>(), Vector2.UnitY * Main.rand.NextFloat(-2, 0), 0, new Color(255, Main.rand.Next(150, 200), 40), Main.rand.NextFloat());
				Dust.NewDustPerfect(pos + new Vector2(965 + Main.rand.Next(-15, 15), 368), DustType<Dusts.Cinder>(), Vector2.UnitY * Main.rand.NextFloat(-2, 0), 0, new Color(255, Main.rand.Next(150, 200), 40), Main.rand.NextFloat());
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			behindNPCsAndTiles.Add(index);
		}

		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Main.myPlayer];

			Vector2 pos = Projectile.position - new Vector2(567, 400) - Main.screenPosition;
			Texture2D backdrop = Request<Texture2D>(AssetDirectory.Glassweaver + "Backdrop").Value;
			Texture2D backdropGlow = Request<Texture2D>(AssetDirectory.Glassweaver + "BackdropGlow").Value;

			Vector2 parallaxOffset = new Vector2(Main.screenPosition.X + Main.screenWidth / 2f - Projectile.position.X, 0) * 0.15f;
			Texture2D farBackdrop = Request<Texture2D>(AssetDirectory.Glassweaver + "FarBackdrop").Value;
			Texture2D farBackdropGlow = Request<Texture2D>(AssetDirectory.Glassweaver + "FarBackdropGlow").Value;

			Texture2D backdropBlack = Request<Texture2D>(AssetDirectory.Glassweaver + "BackdropBlack").Value;

			var frame = new Rectangle(0, 0, backdrop.Width, backdrop.Height);

			LightingBufferRenderer.DrawWithLighting(pos, backdropBlack, frame);

			LightingBufferRenderer.DrawWithLighting(pos + parallaxOffset, farBackdrop, frame);
			Main.spriteBatch.Draw(farBackdropGlow, pos + parallaxOffset, frame, Color.White);

			LightingBufferRenderer.DrawWithLighting(pos, backdrop, frame);
			Main.spriteBatch.Draw(backdropGlow, pos, frame, Color.White);

			float pulse0 = (float)System.Math.Sin(Main.GameUpdateCount * 0.14f) + (float)System.Math.Cos(Main.GameUpdateCount * 0.017f);
			Lighting.AddLight(pos + Main.screenPosition + new Vector2(965, 350), new Vector3(1, 0.8f, 0.4f) * (1.2f + pulse0 * 0.1f));

			float pulse1 = (float)System.Math.Sin(Main.GameUpdateCount * 0.14f + 4) + (float)System.Math.Cos(Main.GameUpdateCount * 0.017f + 2);
			Lighting.AddLight(pos + Main.screenPosition + new Vector2(175, 350), new Vector3(1, 0.8f, 0.4f) * (1.2f + pulse1 * 0.1f));

			Lighting.AddLight(pos + Main.screenPosition + new Vector2(965, 150), new Vector3(1, 0.8f, 0.4f) * 1.2f);
			Lighting.AddLight(pos + Main.screenPosition + new Vector2(175, 150), new Vector3(1, 0.8f, 0.4f) * 1.2f);

			float pulseMiddle = (float)System.Math.Sin(Main.GameUpdateCount * 0.1f) + (float)System.Math.Cos(Main.GameUpdateCount * 0.024f);

			Lighting.AddLight(pos + Main.screenPosition + new Vector2(555, 220), new Vector3(1, 0.6f, 0.4f) * (2f + pulseMiddle * 0.25f));
		}
	}
}