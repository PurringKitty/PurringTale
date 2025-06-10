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
	public class TopiumMask : ModItem
	{
        public static int AdditiveSummonDamageBonus = 10;
        public static int MaxWhipRange = 5;
        public static int MaxMinionIncrease = 5;
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 14;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TopiumBreastplate>() && legs.type == ModContent.ItemType<TopiumLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Summon) += AdditiveSummonDamageBonus / 100f;
            player.maxMinions += MaxMinionIncrease;
            player.whipRangeMultiplier += MaxWhipRange / 100f;

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

