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
	public class ForgedHelmet : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(platinum: 100);
			Item.rare = ItemRarityID.Quest;
			Item.defense = 125;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<ForgedBreastplate>() && legs.type == ModContent.ItemType<ForgedLeggings>();
		}
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Webbed] = true;
        }
        public override void UpdateArmorSet(Player player) 
        {
            player.AddBuff(BuffID.Inferno, 0);
            player.AddBuff(BuffID.Thorns, 0);
            player.moveSpeed += 2f;
            player.statManaMax2 += 500;
            player.maxMinions += 6;
            player.maxTurrets += 6;
            player.statLifeMax2 += 500;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient<RustedHelmet>(1);
            recipe.AddIngredient<TopiumBar>(9999);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();
        }
	}
}
