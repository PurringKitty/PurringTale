using PurringTale.Content.Items.BossDrops;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class GrubbbHead : ModItem
	{


		public static LocalizedText SetBonusText { get; private set; }


		

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
            Item.vanity = true;
            Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 0;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<GrubbbBody>() && legs.type == ModContent.ItemType<GrubbbLegs>();
		
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<VanityVoucher>(1);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();
        }
	}
}
