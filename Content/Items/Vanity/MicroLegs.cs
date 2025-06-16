using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Legs)]
	public class MicroLegs : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18; 
			Item.height = 18;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 1); 
			Item.rare = ItemRarityID.LightPurple;
			ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<VanityVoucher>(1);
            recipe.AddTile<ValhallaWorkbench>();
            recipe.Register();
        }
	}
}