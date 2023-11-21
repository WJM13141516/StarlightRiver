using StarlightRiver.Core.Systems.InstancedBuffSystem;
using StarlightRiver.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;

namespace StarlightRiver.Content.Items.Misc
{
	internal class Release : ModItem
	{
		public override string Texture => AssetDirectory.MiscItem + "Release";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Release");
			Tooltip.SetDefault("Lash out with tendrils of rage\n<RIGHT> to calm yourself and heal\n'Perhaps all you needed was to remember why you fell in love in the first place.'");
		}

		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.DamageType = DamageClass.Magic;
			Item.crit = 10;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<ReleaseTendril>();
			Item.shootSpeed = 1;
			Item.useStyle = Terraria.ID.ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.rare = Terraria.ID.ItemRarityID.Red;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Helpers.Helper.PlayPitched("Magic/FireCast", 0.5f, 0.5f + Main.rand.NextFloat(0.1f), player.Center);
			Projectile.NewProjectile(source, position, velocity.RotatedByRandom(0.2f), type, damage, knockback, player.whoAmI, Main.rand.Next(300, 400), Main.rand.NextBool() ? 1 : -1);
			return false;
		}

		public override bool? UseItem(Player player)
		{
			BuffInflictor.Inflict<ReleaseRage>(player, 300);
			return true;
		}
	}

	internal class ReleaseTendril : ModProjectile, IDrawPrimitive
	{
		private List<Vector2> cache;
		private Trail trail;

		private Vector2 startPoint;
		public Vector2 endPoint;
		public Vector2 midPoint;

		public float dist1;
		public float dist2;

		public override string Texture => AssetDirectory.Invisible;

		public Player Owner => Main.player[Projectile.owner];

		public ref float Length => ref Projectile.ai[0];
		public ref float Direction => ref Projectile.ai[1];

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Projectile.Center = Owner.Center;
			startPoint = Projectile.Center;

			float time = (1 - Helper.BezierEase(Projectile.timeLeft / 40f)) * 6.28f * Direction;
			midPoint = startPoint + new Vector2(Length / 2 + (float)Math.Cos(time) * (Length * 0.4f), (float)Math.Sin(time) * 75).RotatedBy(Projectile.velocity.ToRotation());
			endPoint = startPoint + new Vector2(Length / 2 + (float)Math.Cos(time - 0.5 * Direction) * Length, (float)Math.Sin(time - 0.5) * 120).RotatedBy(Projectile.velocity.ToRotation());

			dist1 = ApproximateSplineLength(30, startPoint, midPoint - startPoint, midPoint, endPoint - startPoint);
			dist2 = ApproximateSplineLength(30, midPoint, endPoint - startPoint, endPoint, endPoint - midPoint);

			var d = Dust.NewDustPerfect(PointOnSpline(Main.rand.Next(20) / 20f), ModContent.DustType<Dusts.AuroraFast>(), Projectile.velocity.RotatedByRandom(0.1f) * Main.rand.NextFloat(20), 0, new Color(255, 0, 0), 1);
			d.customData = Main.rand.NextFloat(0.3f, 0.8f);

			ManageCaches();
			ManageTrail();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return Helper.CheckLinearCollision(startPoint, endPoint, targetHitbox, out _);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;

			var spriteBatch = Main.spriteBatch;
		}

		private Vector2 PointOnSpline(float progress)
		{
			float factor = dist1 / (dist1 + dist2);

			if (progress < factor)
				return Vector2.Hermite(startPoint, midPoint - startPoint, midPoint, endPoint - startPoint, progress * (1 / factor));
			if (progress >= factor)
				return Vector2.Hermite(midPoint, endPoint - startPoint, endPoint, endPoint - midPoint, (progress - factor) * (1 / (1 - factor)));

			return Vector2.Zero;
		}

		private float ApproximateSplineLength(int steps, Vector2 start, Vector2 startTan, Vector2 end, Vector2 endTan)
		{
			float total = 0;
			Vector2 prevPoint = start;

			for (int k = 0; k < steps; k++)
			{
				var testPoint = Vector2.Hermite(start, startTan, end, endTan, k / (float)steps);
				total += Vector2.Distance(prevPoint, testPoint);

				prevPoint = testPoint;
			}

			return total;
		}

		private void ManageCaches()
		{
			if (cache == null)
			{
				cache = new List<Vector2>();

				for (int i = 0; i < 30; i++)
				{
					cache.Add(PointOnSpline(i / 30f));
				}
			}

			for (int i = 0; i < 30; i++)
			{
				cache.Add(PointOnSpline(i / 30f));
			}

			while (cache.Count > 30)
			{
				cache.RemoveAt(0);
			}
		}

		private void ManageTrail()
		{
			trail ??= new Trail(Main.instance.GraphicsDevice, 30, new TriangularTip(40 * 4), factor => (1 - factor) * (Length * 0.15f), factor =>
			{
				float alpha = 1;

				if (Projectile.timeLeft < 10)
					alpha = Projectile.timeLeft / 10f;

				if (Projectile.timeLeft > 30)
					alpha = 1 - (Projectile.timeLeft - 30) / 10f;

				return new Color(255, 75 + (int)((float)Math.Sin(factor.X * 3.14f * 5) * 15), 60) * (float)Math.Sin(factor.X * 3.14f) * alpha;
			});

			trail.Positions = cache.ToArray();
			trail.NextPosition = Projectile.Center + Projectile.velocity;
		}

		public void DrawPrimitives()
		{
			Effect effect = Filters.Scene["CeirosRing"].GetShader().Shader;

			var world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
			Matrix view = Main.GameViewMatrix.TransformationMatrix;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

			effect.Parameters["time"].SetValue(Main.GameUpdateCount * -0.05f);
			effect.Parameters["repeats"].SetValue(2f);
			effect.Parameters["transformMatrix"].SetValue(world * view * projection);
			effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("StarlightRiver/Assets/EnergyTrail").Value);

			trail?.Render(effect);
		}
	}

	internal class ReleaseRage : StackableBuff
	{
		public override string Name => "ReleaseRage";

		public override string DisplayName => "Pure Rage";

		public override string Texture => AssetDirectory.MiscItem + "ReleaseRage";

		public override bool Debuff => true;

		public override BuffStack GenerateDefaultStack(int duration)
		{
			var stack = new BuffStack();
			stack.duration = duration;
			return stack;
		}

		public override void PerStackEffectsPlayer(Player player, BuffStack stack)
		{
			player.GetDamage(DamageClass.Magic) += 0.1f;
			player.lifeRegen -= 1;
		}
	}
}
