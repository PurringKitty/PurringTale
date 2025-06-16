using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using PurringTale.Assets.Textures.Backgrounds;

namespace PurringTale.Assets.Textures.Menu
{
    public class PurringTaleModMenu : ModMenu
    {
        private const string menuAssetPath = "PurringTale/Assets/Textures/Menu";


        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/Title");
        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/THCSun");
        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/THCMoon");


        public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/MainMenu");

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<MainMenuBackgroundStyle>();

        public override string DisplayName => "PurringTale";

        public override void OnSelected()
        {
            SoundEngine.PlaySound(SoundID.Meowmere);
        }
    }
}
