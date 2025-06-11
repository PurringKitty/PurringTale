using PurringTale.Content.Biomes;
using Microsoft.Xna.Framework;
using PurringTale.Content.Dusts;
using Terraria;
using Terraria.ModLoader;

namespace PurringTale.Content.Tiles.Blocks
{
    public class GlitchBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;



            AddMapEntry(new Color(200, 200, 200));
        }


        public override void ChangeWaterfallStyle(ref int style)
        {
            style = ModContent.GetInstance<GlitchWaterfallStyle>().Slot;
        }
    }
}