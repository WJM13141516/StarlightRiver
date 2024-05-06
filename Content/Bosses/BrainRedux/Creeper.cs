﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace StarlightRiver.Content.Bosses.BrainRedux
{
	internal class Creeper : GlobalNPC
	{
		public bool reworked;

		public override bool InstancePerEntity => true;

		public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
		{
			return entity.type == NPCID.Creeper;
		}

		public override bool PreAI(NPC npc)
		{
			if (reworked)
			{
				Lighting.AddLight(npc.Center, new Vector3(0.45f, 0.3f, 0.1f));
			}

			return true;
		}

		public override bool SpecialOnKill(NPC npc)
		{
			return reworked;
		}

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!reworked)
				return true;

			Texture2D glow = ModContent.Request<Texture2D>("StarlightRiver/Assets/Keys/GlowAlpha").Value;
			var color = new Color(180, 100, 40)
			{
				A = 0
			};

			spriteBatch.Draw(glow, npc.Center - Main.screenPosition, null, color * 0.5f, npc.rotation, glow.Size() / 2f, 0.7f, 0, 0);

			return true;
		}
	}
}