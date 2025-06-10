using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using PurringTale.Content.Pets.UFOWolf;
using PurringTale.Content.Pets.Boots;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Accessories.Boots;
using PurringTale.Content.Items.Accessories.Necklaces;
using PurringTale.Content.Items.Weapons.Melee;
using PurringTale.Content.Items.Weapons.Magic;
using PurringTale.Content.Items.Weapons.Summoner;
using PurringTale.Content.Items.Weapons.Ranged;
using PurringTale.Content.Items.Placeables.MusicBoxes;

namespace PurringTale.Common.Systems
{
	public class ChestItemWorldGen : ModSystem
	{
		public override void PostWorldGen() {
			int[] itemsToPlaceInGoldChests = { ModContent.ItemType<PastaNeckless>(), ModContent.ItemType<UFOWolfPetItem>(), ModContent.ItemType<TreadBoots>(), ModContent.ItemType<BootsPetItem>(), ModContent.ItemType<VanityVoucher>(), ModContent.ItemType<THCMusicBox>(), ModContent.ItemType<SnowblindMusicBox>(), ModContent.ItemType<TheLamp>(), ModContent.ItemType<BrokenDestroyerLaserCannon>(), ModContent.ItemType<BrokenStarShootingStaff>(), ModContent.ItemType<BrokenSerratedGreatsword>(), ModContent.ItemType<RuinedGodSlayerSummoningBook>(), ModContent.ItemType<RustedDancersWhip>(), ModContent.ItemType<VanityVoucher>() };
			int itemsToPlaceInGoldChestsChoice = 6;
			int itemsPlaced = 15;
			int maxItems = 150;
			for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if(chest == null) {
					continue;
				}
				Tile chestTile = Main.tile[chest.x, chest.y];
				if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 11 * 36) {
					if (WorldGen.genRand.NextBool(2))
						continue;
					for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++) {
						if (chest.item[inventoryIndex].type == ItemID.None) {
							chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGoldChests[itemsToPlaceInGoldChestsChoice]);
							itemsToPlaceInGoldChestsChoice = (itemsToPlaceInGoldChestsChoice + 1) % itemsToPlaceInGoldChests.Length;
							itemsPlaced++;
							break;
						}
					}
				}
				if(itemsPlaced >= maxItems) {
					break;
				}
			}
		}
	}
}
