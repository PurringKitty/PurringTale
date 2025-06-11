using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles;

namespace PurringTale.Content.Items.Placeables.Blocks
{
    public class GlitchSand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Blocks.GlitchSand>());
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
        }


    }
}