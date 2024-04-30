using System;
using System.IO;
using Terraria.DataStructures;

namespace StarlightRiver.Content.Tiles.Underground.EvasionShrineBullets
{
	class Spear : EvasionProjectile
	{
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_GlowTrail = ModContent.Request<Texture2D>("StarlightRiver/Assets/GlowTrail");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Keys_GlowSoft = ModContent.Request<Texture2D>("StarlightRiver/Assets/Keys/GlowSoft");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Tiles_Moonstone_GlowSmall = ModContent.Request<Texture2D>("StarlightRiver/Assets/Tiles/Moonstone/GlowSmall");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Tiles_Underground_SpearBody = ModContent.Request<Texture2D>("StarlightRiver/Assets/Tiles/Underground/SpearBody");
		public static readonly Asset<Texture2D> texture_Texture___Glow = ModContent.Request<Texture2D>(Texture + "Glow");
		public static Vector2 endPointToAssign;
		public static int riseTimeToAssign;
		public static int retractTimeToAssign;
		public static int teleTimeToASsign;
		public static int holdTimeToAssign;

		public Vector2 startPoint;
		public Vector2 endPoint;
		public int timeToRise;
		public int timeToRetract;
		public int teleTime;
		public int holdTime;
		public EvasionShrineDummy parent;

		public float Alpha => 1 - Projectile.alpha / 255f;

		public override string Texture => AssetDirectory.Assets + "Tiles/Underground/" + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Spear");
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 36;
			Projectile.hostile = true;
			Projectile.timeLeft = 3;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
		}

		public override void OnSpawn(IEntitySource source)
		{
			endPoint = endPointToAssign;
			timeToRise = riseTimeToAssign;
			timeToRetract = retractTimeToAssign;
			teleTime = teleTimeToASsign;
			holdTime = holdTimeToAssign;
		}

		public override void AI()
		{
			if (startPoint == Vector2.Zero)
			{
				Projectile.timeLeft = timeToRise + timeToRetract + teleTime + holdTime;
				startPoint = Projectile.Center;
				Projectile.rotation = (startPoint - endPoint).ToRotation() - 1.57f;
			}

			int timer = timeToRise + timeToRetract + teleTime + holdTime - Projectile.timeLeft;

			if (timer > teleTime && Projectile.alpha > 0)
				Projectile.alpha -= 15;

			if (Projectile.timeLeft < 20)
				Projectile.alpha += 15;

			if (timer < teleTime)
				startPoint = Projectile.Center;
			else if (timer < teleTime + timeToRise)
				Projectile.Center = Vector2.SmoothStep(startPoint, endPoint, (timer - teleTime) / (float)timeToRise);
			else if (timer < teleTime + timeToRise + holdTime)
				Projectile.Center = endPoint;
			else
				Projectile.Center = Vector2.SmoothStep(endPoint, startPoint, (timer - timeToRise - teleTime - holdTime) / (float)timeToRetract);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			int timer = timeToRise + timeToRetract + teleTime + holdTime - Projectile.timeLeft;
			bool line = Collision.CheckAABBvLineCollision(targetHitbox.Location.ToVector2(), targetHitbox.Size(), startPoint, Projectile.Center);

			if (line && timer > teleTime)
				return true;

			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D glowTex = texture_Texture___Glow.Value;
			Main.spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, null, new Color(100, 0, 255) * Alpha, Projectile.rotation, glowTex.Size() / 2, 1, 0, 0);

			float dist = Vector2.Distance(Projectile.Center, startPoint);
			Texture2D bodyTex = texture_StarlightRiver_Assets_Tiles_Underground_SpearBody.Value;

			for (int k = bodyTex.Height; k < dist; k += bodyTex.Height)
			{
				var pos = Vector2.Lerp(Projectile.Center, startPoint, k / dist);
				Main.spriteBatch.Draw(bodyTex, pos - Main.screenPosition, null, new Color(100, 0, 255) * Alpha, Projectile.rotation, bodyTex.Size() / 2, 1, 0, 0);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;

			spriteBatch.End();
			spriteBatch.Begin(default, BlendState.Additive, Main.DefaultSamplerState, default, RasterizerState.CullNone, default, Main.GameViewMatrix.TransformationMatrix);

			int timer = timeToRise + timeToRetract + teleTime + holdTime - Projectile.timeLeft;

			if (timer > teleTime)
			{
				Texture2D tex = texture_StarlightRiver_Assets_Tiles_Moonstone_GlowSmall.Value;
				Texture2D tex2 = texture_StarlightRiver_Assets_Keys_GlowSoft.Value;

				float opacity;

				if (timer < teleTime + timeToRise)
					opacity = (timer - teleTime) / (float)timeToRise;
				else if (timer < teleTime + timeToRise + holdTime)
					opacity = 1;
				else
					opacity = 1 - (timer - timeToRise - teleTime - holdTime) / (float)timeToRetract;

				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(150, 50, 255), Projectile.rotation + 3.14f, new Vector2(tex.Width / 2, 70), 2.8f * opacity, 0, 0);
				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + 3.14f, new Vector2(tex.Width / 2, 70), 2.2f * opacity, 0, 0);

				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(100, 0, 255), Projectile.rotation, new Vector2(tex.Width / 2, 60), 2.2f * opacity, 0, 0);
				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, 60), 1.5f * opacity, 0, 0);

				spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, new Color(150, 50, 255) * opacity, Projectile.rotation, tex2.Size() / 2, 1.5f, 0, 0);
			}
			else
			{
				Texture2D tex = texture_StarlightRiver_Assets_GlowTrail.Value;
				float opacity = (float)Math.Sin(timer / (float)teleTime * 3.14f) * 0.5f;

				Vector2 pos = Projectile.Center - Main.screenPosition;
				float dist = Vector2.Distance(Projectile.Center, endPoint) - 4;
				var target = new Rectangle((int)pos.X, (int)pos.Y, (int)dist, 40);

				spriteBatch.Draw(tex, target, null, new Color(150, 150, 155) * opacity * 0.5f, Projectile.rotation + 1.57f, new Vector2(tex.Width, tex.Height / 2), 0, 0);
				target.Height = 16;
				spriteBatch.Draw(tex, target, null, Color.White * opacity * 0.7f, Projectile.rotation + 1.57f, new Vector2(tex.Width, tex.Height / 2), 0, 0);

				Texture2D tex2 = texture_Texture___Glow.Value;
				spriteBatch.Draw(tex2, endPoint - Main.screenPosition, null, Color.White * opacity, Projectile.rotation, new Vector2(tex2.Width / 2, tex2.Height - 5), 1.0f, 0, 0);
			}

			spriteBatch.End();
			spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, RasterizerState.CullNone, default, Main.GameViewMatrix.TransformationMatrix);

			return true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WritePackedVector2(endPoint);
			writer.Write(timeToRise);
			writer.Write(timeToRetract);
			writer.Write(teleTime);
			writer.Write(holdTime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			endPoint = reader.ReadPackedVector2();
			timeToRise = reader.ReadInt32();
			timeToRetract = reader.ReadInt32();
			teleTime = reader.ReadInt32();
			holdTime = reader.ReadInt32();
		}
	}
}