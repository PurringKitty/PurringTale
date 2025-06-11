using PurringTale.Content.Tiles.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Blocks
{
    internal class GlitchSappling : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<GlitchSapling>());
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.Green;
        }
    }
}
