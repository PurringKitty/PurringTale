using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class TopiumLeggings : ModItem
	{
        public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 25;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.legSlot] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient<TopiumBar>(15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
