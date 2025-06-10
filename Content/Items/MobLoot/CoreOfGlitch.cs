using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables.Blocks;

namespace PurringTale.Content.Items.MobLoot;

public class CoreOfGlitch: ModItem
{


    public override void SetDefaults()
    {
        Item.width = 15;
        Item.height = 15;
        Item.maxStack = 9999;
        Item.material = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 10);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
    }

    public override void AddRecipes()
    {
       Recipe gc = CreateRecipe(2);
        gc.AddIngredient(ModContent.ItemType<GlitchBlock>());
        gc.AddTile(TileID.WorkBenches);
        gc.Register();
    }
}
