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
	public class PastasHood : ModItem
	{
        
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<PastasHoodie>() && legs.type == ModContent.ItemType<PastasLeggings>();
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

