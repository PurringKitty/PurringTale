using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using PurringTale.Content.Dusts;

namespace PurringTale.Content.Biomes
{
    public class GlitchWaterStyle : ModWaterStyle
    {
        private Asset<Texture2D> rainTexture;
        public override void Load()
        {
            rainTexture = Mod.Assets.Request<Texture2D>("Content/Biomes/GlitchRain");
        }

        public override int ChooseWaterfallStyle()
        {
            return ModContent.GetInstance<GlitchWaterfallStyle>().Slot;
        }

        public override int GetSplashDust()
        {
            return ModContent.DustType<GlitchSolution>();
        }

        public override int GetDropletGore()
        {
            return ModContent.GoreType<GlitchDroplet>();
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 1f;
            g = 1f;
            b = 1f;
        }

        public override Color BiomeHairColor()
        {
            return Color.DarkGreen;
        }

        public override byte GetRainVariant()
        {
            return (byte)Main.rand.Next(3);
        }

        public override Asset<Texture2D> GetRainTexture() => rainTexture;
    }
}