﻿using ReLogic.Utilities;
using StarlightRiver.Content.Bosses.BrainRedux;
using StarlightRiver.Content.Buffs;
using StarlightRiver.Content.CustomHooks;
using StarlightRiver.Content.Tiles.Crimson;
using StarlightRiver.Core.Systems.ScreenTargetSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.IO;

namespace StarlightRiver.Content.Biomes
{
	internal class GraymatterBiome : ModBiome
	{
		public static bool forceGrayMatter;
		public static int forceTimer;

		public List<Vector2> thinkerPositions;

		public Vector2 lastGrayPos;

		public static ScreenTarget hallucinationMap;
		public static ScreenTarget overHallucinationMap;

		/// <summary>
		/// Can be subscribed to to add additional hallucinatory areas to the screen
		/// </summary>
		public static Action<SpriteBatch> onDrawHallucinationMap;

		/// <summary>
		/// Can be subscribed to to add additional 'hallucinatory' objects that can only be seen in a hallucinatory area
		/// </summary>
		public static Action<SpriteBatch> onDrawOverHallucinationMap;

		/// <summary>
		/// Can be subscribed to for drawing hallucinatory tiles, seperated to be able to be called from one iteration
		/// for optimization.
		/// </summary>
		public static Action<SpriteBatch, int, int> onDrawOverPerTile;

		public static int fullscreenTimer = 0;

		public override void Load()
		{
			hallucinationMap = new(DrawHallucinationMap, () => IsBiomeActive(Main.LocalPlayer), 1);
			overHallucinationMap = new(DrawOverHallucinationMap, () => IsBiomeActive(Main.LocalPlayer), 1.1f);

			Screenspace.DrawScreenspaceEvent += DrawAuras;
		}

		public override bool IsBiomeActive(Player player)
		{
			return forceGrayMatter || forceTimer > 0 || fullscreenTimer > 0 || ModContent.GetInstance<GraymatterBiomeSystem>().anyTiles;
		}

		public override void OnInBiome(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				if (player.HasBuff(ModContent.BuffType<CrimsonHallucination>()))
				{
					lastGrayPos = player.Center;
					if (fullscreenTimer < 40)
						fullscreenTimer++;
				}
				else if (fullscreenTimer > 0)
				{
					fullscreenTimer--;
				}
			}
		}

		public void DrawHallucinationMap(SpriteBatch spriteBatch)
		{
			onDrawHallucinationMap?.Invoke(spriteBatch);

			// Draw the screen overlay for when the player is actively standing on gray matter
			Texture2D glow = Assets.Keys.GlowAlpha.Value;
			spriteBatch.Draw(glow, lastGrayPos - Main.screenPosition, null, new Color(1, 1, 1f, 0), 0, glow.Size() / 2f, Main.screenWidth / glow.Width * (fullscreenTimer / 9f), 0, 0);
			spriteBatch.Draw(glow, lastGrayPos - Main.screenPosition, null, new Color(1, 1, 1f, 0), 0, glow.Size() / 2f, Main.screenWidth / glow.Width * (fullscreenTimer / 9f), 0, 0);
		}

		public void DrawOverHallucinationMap(SpriteBatch spriteBatch)
		{
			onDrawOverHallucinationMap?.Invoke(spriteBatch);

			var pos = (Main.screenPosition / 16).ToPoint16();

			int width = Main.screenWidth / 16 + 1;
			int height = Main.screenHeight / 16 + 1;

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Point16 target = pos + new Point16(x, y);
					onDrawOverPerTile.Invoke(spriteBatch, target.X, target.Y);
				}
			}
		}

		private void DrawAuras(SpriteBatch spriteBatch)
		{
			if (IsBiomeActive(Main.LocalPlayer))
			{
				Effect shader = Filters.Scene["GrayMatter"].GetShader().Shader;
				Texture2D noise = ModContent.Request<Texture2D>("StarlightRiver/Assets/Noise/SwirlyNoiseLooping").Value;

				shader.Parameters["background"].SetValue(Main.screenTarget);
				shader.Parameters["map"].SetValue(hallucinationMap.RenderTarget);
				shader.Parameters["noise"].SetValue(noise);
				shader.Parameters["over"].SetValue(overHallucinationMap.RenderTarget);
				shader.Parameters["time"].SetValue(Main.GameUpdateCount * 0.02f);
				shader.Parameters["screensize"].SetValue(noise.Size() / new Vector2(Main.screenWidth, Main.screenHeight));

				spriteBatch.End();
				spriteBatch.Begin(default, default, SamplerState.PointWrap, default, default, shader, Main.GameViewMatrix.TransformationMatrix);

				spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);

				spriteBatch.End();
				spriteBatch.Begin(default, default, SamplerState.PointWrap, default, default);
			}
		}
	}

	internal class GraymatterBiomeSystem : ModSystem
	{
		public bool anyTiles;
		public bool reset;

		public List<Vector2> thinkerPositions = new();

		public override void Load()
		{
			On_WorldGen.CrimVein += AddThinker;
		}

		private void AddThinker(On_WorldGen.orig_CrimVein orig, Vector2D position, Vector2D velocity)
		{
			if (!thinkerPositions.Contains(position.ToPoint().ToVector2()))
				thinkerPositions.Add(position.ToPoint().ToVector2());

			orig(position, velocity);
		}

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
		{
			anyTiles = tileCounts[ModContent.TileType<GrayMatter>()] > 0;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("ThinkerPositions", thinkerPositions);
		}

		public override void ClearWorld()
		{
			reset = false;
			TheThinker.toRender.Clear();
		}

		public override void PreUpdateEntities()
		{
			if (GraymatterBiome.forceGrayMatter)
				GraymatterBiome.forceTimer = 4;

			if (GraymatterBiome.forceTimer > 0)
				GraymatterBiome.forceTimer--;

			GraymatterBiome.forceGrayMatter = false;
		}

		public override void PostUpdateEverything()
		{
			if (!reset)
			{
				for (int k = 0; k < Main.maxNPCs; k++)
				{
					if (Main.npc[k].ModNPC is TheThinker thinker)
					{
						if (thinker.active)
							thinker.ResetArena();
					}
				}

				reset = true;
			}
		}

		public override void LoadWorldData(TagCompound tag)
		{
			thinkerPositions = tag.GetList<Vector2>("ThinkerPositions") as List<Vector2>;

			foreach (Vector2 pos in thinkerPositions)
			{
				if (!Main.npc.Any(n => n.active && n.type == ModContent.NPCType<TheThinker>() && Vector2.Distance(n.Center, pos) < 64))
					NPC.NewNPC(null, (int)pos.X * 16, (int)pos.Y * 16, ModContent.NPCType<TheThinker>());
			}
		}
	}
}