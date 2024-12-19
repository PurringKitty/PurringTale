using PurringTale.Content.Items.Vanity;
using PurringTale.Content.NPCs.BossNPCs;
using PurringTale.Content.Items.BossDrops;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Pets.Boots;
using PurringTale.CatBoss;

namespace PurringTale.Content.Items.Consumables
{
	// Basic code for a boss treasure bag
	public class THGBossBag : ModItem
	{
		public override void SetStaticDefaults() {
			// This set is one that every boss bag should have.
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.value = 1000;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			// We have to replicate the expert drops from MinionBossBody here

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopHatDemonEye>(), 8, 1, 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopHatDemonPendent>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonlightGreatSword>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EldritchBlaster>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook1>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook2>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook3>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldBook4>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BootsPetItem>(), 8, 1, 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TopiumOre>(), 2, 1, 100));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<TopHatCatBoss>()));
        }
	}
}
