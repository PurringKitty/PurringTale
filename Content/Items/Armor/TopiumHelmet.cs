using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class TopiumHelmet : ModItem
	{
        public static readonly int AdditiveMeleeDamageBonus = 10;
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100);
			Item.rare = ItemRarityID.Cyan; 
			Item.defense = 28; 
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TopiumBreastplate>() && legs.type == ModContent.ItemType<TopiumLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Melee) += AdditiveMeleeDamageBonus / 100f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient<TopiumBar>(20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
}

