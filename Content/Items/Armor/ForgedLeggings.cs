using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ForgedLeggings : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(platinum: 100);
			Item.rare = ItemRarityID.Quest;
			Item.defense = 100;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<RustedLeggings>(1);
            recipe.AddIngredient<TopiumBar>(9999);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();
        }
	}
}
