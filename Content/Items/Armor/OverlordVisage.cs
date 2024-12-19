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
	public class OverlordVisage : ModItem
	{
        public override void SetDefaults() {
			Item.width = 32; // Width of the item
			Item.height = 32; // Height of the item
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.LightPurple; // The rarity of the item
			Item.defense = 10; // The amount of defense the item will give when equipped
		}

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<OverlordChestplate>() && legs.type == ModContent.ItemType<OverlordBoots>();
        }




                    public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f; // Increase dealt damage for all weapon classes by 20%
        
        }
                    public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 10);
            recipe.AddIngredient<CoreOfValhalla>(10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
}

