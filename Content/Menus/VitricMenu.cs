using ReLogic.Content;
using StarlightRiver.Content.CustomHooks;

namespace StarlightRiver.Content.Menus
{
	internal class VitricMenu : ModMenu
	{
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Misc_MenuIconGlow = ModContent.Request<Texture2D>("StarlightRiver/Assets/Misc/MenuIconGlow");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Misc_MenuIconGlow2 = ModContent.Request<Texture2D>("StarlightRiver/Assets/Misc/MenuIconGlow2");
		public static readonly Asset<Texture2D> texture_StarlightRiver_Assets_Misc_MenuIcon = ModContent.Request<Texture2D>("StarlightRiver/Assets/Misc/MenuIcon");
		public override string DisplayName => "Vitric";
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/GlassPassive");

		public override Asset<Texture2D> Logo => texture_StarlightRiver_Assets_Misc_MenuIcon;

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			logoScale = 1.0f;

			VitricBackground.DrawTitleVitricBackground();
			return true;
		}

		public override void PostDrawLogo(SpriteBatch spriteBatch, Vector2 logoDrawCenter, float logoRotation, float logoScale, Color drawColor)
		{
			Texture2D tex2 = texture_StarlightRiver_Assets_Misc_MenuIconGlow2.Value;
			Color color = Color.White;
			color.A = 0;
			spriteBatch.Draw(tex2, logoDrawCenter, null, color, logoRotation, tex2.Size() / 2f, logoScale, 0, 0);

			Texture2D tex = texture_StarlightRiver_Assets_Misc_MenuIconGlow.Value;
			spriteBatch.Draw(tex, logoDrawCenter, null, Color.White, logoRotation, tex.Size() / 2f, logoScale, 0, 0);
		}
	}
}