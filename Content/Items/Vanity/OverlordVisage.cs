using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class OverlordVisage : ModItem
	{
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<OverlordChestplate>() && legs.type == ModContent.ItemType<OverlordBoots>();
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

