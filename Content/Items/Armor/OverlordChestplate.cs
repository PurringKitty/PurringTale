using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class OverlordChestplate : ModItem
    	{

        public override void SetDefaults() {
			Item.width = 28; // Width of the item
			Item.height = 32; // Height of the item
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.LightPurple; // The rarity of the item
			Item.defense = 12; // The amount of defense the item will give when equipped

    }
                   public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 15);
            recipe.AddIngredient<CoreOfValhalla>(10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();


        }
	}
}
