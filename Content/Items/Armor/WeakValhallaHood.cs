﻿using PurringTale.Content.Items.MobLoot;
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
	public class WeakValhallaHood : ModItem
	{
        public static readonly int AdditiveMagicDamageBonus = 5;
        public static int MaxManaIncrease = 50;

        public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            // If your head equipment should draw hair while drawn, use one of the following:
            // ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
            // ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
            // ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
            // ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveMagicDamageBonus);
        }




        public override void SetDefaults() {
			Item.width = 32; // Width of the item
			Item.height = 32; // Height of the item
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.LightPurple; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WeakValhallaBreastplate>() && legs.type == ModContent.ItemType<WeakValhallaLeggings>();
        }




                    public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value; // This is the setbonus tooltip: "Increases dealt damage by 20%"
            player.GetDamage(DamageClass.Generic) += AdditiveMagicDamageBonus / 5f; // Increase dealt damage for all weapon classes by 20%
            player.statManaMax2 += MaxManaIncrease; // Increase how many mana points the player can have by 20
        }
                    public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient<WeakValhallaBar>(20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
}

