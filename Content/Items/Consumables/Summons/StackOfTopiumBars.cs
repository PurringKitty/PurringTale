using PurringTale.CatBoss;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.NPCs;
using PurringTale.Content.NPCs.TownNPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Consumables.Summons
{
	public class StackOfTopiumBars : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 1;
			Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) 
		{
			return !NPC.AnyNPCs(ModContent.NPCType<TopHatCatBoss>());
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				SoundEngine.PlaySound(SoundID.Meowmere, player.position);

				int type = ModContent.NPCType<TopHatCatBoss>();

				if (Main.netMode != NetmodeID.MultiplayerClient) 
				{
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else 
				{
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ClumpOfTopiumOre>(1)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}