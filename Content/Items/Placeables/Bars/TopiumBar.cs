using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Tiles.Bars;

namespace PurringTale.Content.Items.Placeables.Bars;

public class TopiumBar : ModItem
{


    public override void SetDefaults()
    {
        Item.width = 15;
        Item.height = 12;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.value = Item.sellPrice(gold: 100);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Master;
        Item.ResearchUnlockCount = 100;
        Item.createTile = ModContent.TileType<TopiumBlock>();
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<TopiumOre>(10)
        .AddTile(TileID.AdamantiteForge)
        .Register();
    }
}