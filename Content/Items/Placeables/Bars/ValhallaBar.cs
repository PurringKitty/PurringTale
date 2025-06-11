using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Ores;
using PurringTale.Content.Tiles.Bars;

namespace PurringTale.Content.Items.Placeables.Bars;
public class ValhallaBar : ModItem
{


    public override void SetDefaults()
    {
        Item.width = 15;
        Item.height = 12;
        Item.maxStack = 9999;
        Item.consumable = true;
        Item.value = Item.sellPrice(silver: 1);
        Item.createTile = ModContent.TileType<ValhallaBlock>();
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
        .AddIngredient<ValhallaOre>(5)
        .AddIngredient<CoreOfValhalla>(1)
        .AddTile(TileID.Furnaces)
        .Register();
    }
}