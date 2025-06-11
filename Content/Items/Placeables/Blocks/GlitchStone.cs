using PurringTale.Content.Items.MobLoot;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Placeables.Blocks
{
    internal class GlitchStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Blocks.GlitchBlock>());
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe gs = CreateRecipe();
            gs.AddIngredient(ModContent.ItemType<CoreOfGlitch>(), 2);
            gs.AddIngredient(ItemID.StoneBlock);
            gs.AddTile(TileID.WorkBenches);
            gs.Register();
        }
    }
}
