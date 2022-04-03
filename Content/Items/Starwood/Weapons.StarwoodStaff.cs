using Microsoft.Xna.Framework;
using StarlightRiver.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarlightRiver.Content.Items.Starwood
{
	public class StarwoodStaff : StarwoodItem
    {
        public override string Texture => AssetDirectory.StarwoodItem + Name;

        public StarwoodStaff() : base(ModContent.Request<Texture2D>(AssetDirectory.StarwoodItem + "StarwoodStaff_Alt").Value) { }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starwood Staff");
            Tooltip.SetDefault("Creates a burst of small stars\nStriking an enemy with every star causes a larger star to drop on them");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.magic = true;
            Item.mana = 10;
            Item.width = 18;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item43;
            Item.knockBack = 0f;
            Item.shoot = ModContent.ProjectileType<StarwoodStaffProjectile>();
            Item.shootSpeed = 15f;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player Player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            StarlightPlayer mp = Main.player[Player.whoAmI].GetModPlayer<StarlightPlayer>();
            int amount = mp.empowered ? 4 : 3;
            int projDamage = (int)(damage * (mp.empowered ? 1.3f : 1f));//TODO: actually change the Item itself's damage
            float projSpeedX = speedX * (mp.empowered ? 1.05f : 1f);
            float projSpeedY = speedY * (mp.empowered ? 1.05f : 1f);

            Vector2 staffEndPosition = Player.Center + Vector2.Normalize(Main.MouseWorld - position) * 45;//this makes it spawn a distance from the Player, useful for other stuff

            for (int k = 0; k < amount; k++)
                Projectile.NewProjectile(staffEndPosition, new Vector2(projSpeedX, projSpeedY).RotatedBy(Main.rand.NextFloat(-0.05f, 0.05f) * (k * 0.10f + 1)) * Main.rand.NextFloat(0.9f, 1.1f) * (k * 0.15f + 1), type, projDamage, knockBack, Player.whoAmI, Main.rand.NextFloat(-0.025f, 0.025f), Main.rand.Next(50));

            for (int k = 0; k < 10; k++)
                Dust.NewDustPerfect(staffEndPosition + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-5f, 15f)), mp.empowered ? ModContent.DustType<Dusts.BlueStamina>() : ModContent.DustType<Dusts.Stamina>(), (new Vector2(projSpeedX, projSpeedY) * Main.rand.NextFloat(0.01f, 0.1f)).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) + Player.velocity * 0.5f, 0, default, 1.5f);
            return false;
        }

    }
}