using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DirtLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(copper: 5);
            Item.rare = ItemRarityID.Gray;
            Item.defense = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 25)
                .AddTile<Tiles.Furniture.ValhallaWorkbench>()
                .Register();
        }
    }
}
