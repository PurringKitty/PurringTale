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
	public class TopiumCap : ModItem
	{
        public static int AdditiveSummonDamageBonus = 10;
        public static int MaxSentryIncrease = 2;
        public static int MaxWhipRange = 5;
        public static int MaxMinionIncrease = 5;
       

        public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            // If your head equipment should draw hair while drawn, use one of the following:
            // ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
            // ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
            // ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
            // ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveSummonDamageBonus);
        }




        public override void SetDefaults() {
			Item.width = 32; // Width of the item
			Item.height = 32; // Height of the item
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100); // How many coins the item is worth
			Item.rare = ItemRarityID.Cyan; // The rarity of the item
			Item.defense = 14; // The amount of defense the item will give when equipped
		}

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TopiumBreastplate>() && legs.type == ModContent.ItemType<TopiumLeggings>();
        }




                    public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value; // This is the setbonus tooltip: "Increases dealt damage by 20%"
            player.GetDamage(DamageClass.Summon) += AdditiveSummonDamageBonus / 30f; // Increase dealt damage for all weapon classes by 20%
            player.maxMinions += MaxMinionIncrease; // Increase how many minions the player can have by one        
            player.maxMinions += MaxSentryIncrease; // Increase how many minions the player can have by one
            player.whipRangeMultiplier += MaxWhipRange / 10f;

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
