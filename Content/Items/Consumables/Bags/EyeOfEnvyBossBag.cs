using PurringTale.Content.Items.Vanity;
using PurringTale.Content.NPCs.BossNPCs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.NPCs.TownNPCs;
using PurringTale.Content.Items.Accessories.Emblems;
using PurringTale.Content.Items.Placeables.Ores;

namespace PurringTale.Content.Items.Consumables.Bags
{
	// Basic code for a boss treasure bag
	public class EyeOfEnvyBossBag : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Purple;
			Item.expert = true;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<WeakValhallaOre>(), 4, 1, 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnvyousOre>(), 1, 50, 200));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Envy>(), 1, 1, 1));
        }
	}
}
