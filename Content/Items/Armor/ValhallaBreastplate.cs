using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
	public class ValhallaBreastplate : ModItem
    	{
        public override void SetDefaults() {
			Item.width = 28;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddIngredient<ValhallaBar>(25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
