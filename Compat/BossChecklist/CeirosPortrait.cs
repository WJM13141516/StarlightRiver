using System;

namespace StarlightRiver.Compat.BossChecklist
{
	class CeirosPortrait
	{
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Keys_GlowSoft = ModContent.Request<Texture2D>("StarlightRiver/Assets/Keys/GlowSoft");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Keys_Glow = ModContent.Request<Texture2D>("StarlightRiver/Assets/Keys/Glow");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_BossChecklist_VitricBossGlow = ModContent.Request<Texture2D>("StarlightRiver/Assets/BossChecklist/VitricBossGlow");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_BossChecklist_VitricBoss = ModContent.Request<Texture2D>("StarlightRiver/Assets/BossChecklist/VitricBoss");
		private readonly static ParticleSystem ceirosSystem = new("StarlightRiver/Assets/Keys/GlowSoft", n =>
		{
			n.Velocity.X = (float)Math.Sin(n.Timer / 10f);
			n.Velocity *= 0.975f;
			n.Position += n.Velocity;
			n.Scale *= 0.99f;

			n.Color = Color.Lerp(Color.Red, Color.Yellow, n.Timer / 100f) * (float)Math.Sin(n.Timer / 100f * 3.14f);

			n.Timer--;
		}, ParticleSystem.AnchorOptions.UI);

		public static void DrawCeirosPortrait(SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			if (Main.rand.NextBool(2))
				ceirosSystem.AddParticle(new Particle(rect.Center() + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(100), Vector2.UnitY * -3, 0, Main.rand.NextFloat(0.3f), new Color(200, 200, 0), 100, Vector2.Zero));

			float sin = 0.6f + (float)Math.Sin(Main.GameUpdateCount / 100f) * 0.2f;

			Texture2D tex0 = texture_StarlightRiver_Assets_BossChecklist_VitricBoss.Value;
			Texture2D tex = texture_StarlightRiver_Assets_BossChecklist_VitricBossGlow.Value;
			Texture2D tex2 = texture_StarlightRiver_Assets_Keys_Glow.Value;
			spriteBatch.Draw(tex2, rect, null, Color.Black * 0.6f, 0, Vector2.UnitY * 2, 0, 0);

			spriteBatch.End();
			spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, default, default, Main.UIScaleMatrix);

			spriteBatch.Draw(tex, rect.Center(), null, Color.White * sin, 0, tex.Size() / 2, 1, 0, 0);
			ceirosSystem.DrawParticles(spriteBatch);

			spriteBatch.End();
			spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, default, default, Main.UIScaleMatrix);

			spriteBatch.Draw(tex0, rect.Center(), null, color, 0, tex0.Size() / 2, 1, 0, 0);

			ceirosSystem.SetTexture(texture_StarlightRiver_Assets_Keys_GlowSoft.Value);
		}
	}
}