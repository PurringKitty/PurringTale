using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Others
{ 
    [AutoloadEquip(EquipType.Back)]


    public class TheCubeGeneric : ModItem
    {
 public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(platinum: 50);
            Item.accessory = true;
            Item.rare = ItemRarityID.Quest;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar,100);
            recipe.AddIngredient<TopiumBar>(99999);
            recipe.AddIngredient<ValhallaBar>(99999);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 20;
            player.statManaMax2 += 200;
            player.statLifeMax2 += 200;
        }
    }
}