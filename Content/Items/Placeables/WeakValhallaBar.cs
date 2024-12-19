using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Placeables;
public class WeakValhallaBar : ModItem
{


    public override void SetDefaults()
    {
        Item.width = 15;
        Item.height = 12;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.value = Item.sellPrice(silver: 50);
        Item.createTile = ModContent.TileType<Content.Tiles.WeakValhallaBlock>();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.ResearchUnlockCount = 100;
        Item.rare = ItemRarityID.Master;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
        .AddIngredient<WeakValhallaOre>(5)
        .AddIngredient<CoreOfValhalla>(2)
        .AddTile(TileID.Furnaces)
        .Register();
    }
}