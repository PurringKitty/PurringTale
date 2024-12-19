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
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class GodSlayerHelmet : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 100); // How many coins the item is worth
			Item.rare = ItemRarityID.Red; // The rarity of the item
			Item.defense = 125; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<GodSlayerBreastplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();
		}
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true; // Make the player immune to Fire
            player.GetDamage(DamageClass.Generic) += 1.10f;
        }
        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player) {
            player.AddBuff(BuffID.Wrath, 100);
            player.AddBuff(BuffID.Inferno, 100);
            player.AddBuff(BuffID.Lifeforce, 100);
            player.AddBuff(BuffID.Endurance, 100);
            player.AddBuff(BuffID.Gills, 100);
            player.AddBuff(BuffID.Titan, 100);
            player.AddBuff(BuffID.Panic, 100);
            player.AddBuff(BuffID.Thorns, 100);
            player.AddBuff(BuffID.Hunter, 100);
            player.AddBuff(BuffID.Endurance, 100);
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient<OldGodSlayerHelmet>(1);
            recipe.AddIngredient<TopiumBar>(9999);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
	}
}
