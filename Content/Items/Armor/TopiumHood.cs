using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class TopiumHood : ModItem
	{
        public static readonly int AdditiveMagicDamageBonus = 15;
        public static int MaxManaIncrease = 100;
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 18;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TopiumBreastplate>() && legs.type == ModContent.ItemType<TopiumLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Magic) += AdditiveMagicDamageBonus / 100f;
            player.statManaMax2 += MaxManaIncrease;
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

