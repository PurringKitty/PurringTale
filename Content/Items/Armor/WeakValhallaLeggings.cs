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
	public class WeakValhallaLeggings : ModItem
	{
        public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 5;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.legSlot] = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient<WeakValhallaBar>(15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
