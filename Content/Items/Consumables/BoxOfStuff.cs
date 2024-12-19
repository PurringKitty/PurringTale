using PurringTale.Content.NPCs.BossNPCs;
using PurringTale.Content.Items.BossDrops;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.NPCs.TownNPCs;
using PurringTale.Content.Items.Accessories;
using PurringTale.Content.Pets.Boots;
using PurringTale.Content.Pets.UFOWolf;
using PurringTale.Content.Items.Armor;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Items.Tools;
using System.Collections.Generic;
using PurringTale.Content.Pets.FedoraCat;

namespace PurringTale.Content.Items.Consumables
{
	
	public class BoxOfStuff : ModItem
	{
		public override void SetStaticDefaults() 
        {
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults() {
			Item.maxStack = 1;
			Item.consumable = true;
			Item.width = 36;
			Item.height = 44;
            Item.value = 0;
            Item.rare = ItemRarityID.Master;
		}
		public override bool CanRightClick() {
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PastaNeckless>(), 4, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartOfSlime>(), 10, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TreadBoots>(), 4, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BootsPetItem>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<UFOWolfPetItem>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FedoraCatPetItem>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtHelmet>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtBreastplate>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtLeggings>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtSword>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtPickaxe>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DirtAxe>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfEnvy>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfGluttony>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfGreed>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfLust>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfPride>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfSloth>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BowlOfWrath>(), 15, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StackOfTopiumBars>(), 15, 1, 1));
        }
    }
}


