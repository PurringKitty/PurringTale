using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using PurringTale.Content.Items.Accessories;
using PurringTale.Content.Pets;
using PurringTale.Content.Pets.UFOWolf;
using PurringTale.Content.Pets.Boots;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Items.BossDrops;

namespace PurringTale.Common.Systems
{
	// This class showcases adding additional items to vanilla chests.
	// This example simply adds additional items. More complex logic would likely be required for other scenarios.
	// If this code is confusing, please learn about "for loops" and the "continue" and "break" keywords: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/jump-statements
	public class ChestItemWorldGen : ModSystem
	{
		// We use PostWorldGen for this because we want to ensure that all chests have been placed before adding items.
		public override void PostWorldGen() {
			// Place some additional items in Frozen Chests:
			// These are the 3 new items we will place.
			int[] itemsToPlaceInGoldChests = { ModContent.ItemType<PastaNeckless>(), ModContent.ItemType<UFOWolfPetItem>(), ModContent.ItemType<TreadBoots>(), ModContent.ItemType<BootsPetItem>(), ModContent.ItemType<VanityVoucher>(), ModContent.ItemType<THCMusicBox>(), ModContent.ItemType<SnowblindMusicBox>(), ModContent.ItemType<TheLamp>(), ModContent.ItemType<BrokenDestroyerLaserCannon>(), ModContent.ItemType<BrokenStarShootingStaff>(), ModContent.ItemType<BrokenSerratedGreatsword>(), ModContent.ItemType<RuinedGodSlayerSummoningBook>(), ModContent.ItemType<RustedDancersWhip>(), ModContent.ItemType<VanityVoucher>() };
			// This variable will help cycle through the items so that different Frozen Chests get different items
			int itemsToPlaceInGoldChestsChoice = 0;
			// Rather than place items in each chest, we'll place up to 6 items (2 of each). 
			int itemsPlaced = 0;
			int maxItems = 150;
			// Loop over all the chests
			for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if(chest == null) {
					continue;
				}
				Tile chestTile = Main.tile[chest.x, chest.y];
				// We need to check if the current chest is the Frozen Chest. We need to check that it exists and has the TileType and TileFrameX values corresponding to the Frozen Chest.
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Frozen Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. An alternate approach is to check the wiki and looking for the "Internal Tile ID" section in the infobox: https://terraria.wiki.gg/wiki/Frozen_Chest
				if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 11 * 36) {
					// We have found a Frozen Chest
					// If we don't want to add one of the items to every Frozen Chest, we can randomly skip this chest with a 33% chance.
					if (WorldGen.genRand.NextBool(2))
						continue;
					// Next we need to find the first empty slot for our item
					for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++) {
						if (chest.item[inventoryIndex].type == ItemID.None) {
							// Place the item
							chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGoldChests[itemsToPlaceInGoldChestsChoice]);
							// Decide on the next item that will be placed.
							itemsToPlaceInGoldChestsChoice = (itemsToPlaceInGoldChestsChoice + 1) % itemsToPlaceInGoldChests.Length;
							// Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.Next(itemsToPlaceInFrozenChests));
							itemsPlaced++;
							break;
						}
					}
				}
				// Once we've placed as many items as we wanted, break out of the loop
				if(itemsPlaced >= maxItems) {
					break;
				}
			}
		}
	}
}
