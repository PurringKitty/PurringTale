using PurringTale.Content.Items.Placeables;
using PurringTale.Content.NPCs;
using PurringTale.Content.NPCs.BossNPCs.Envy;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.NPCs.TownNPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.NPCs.BossNPCs.ZeRock;

namespace PurringTale.Content.Items.Consumables.Summons
{
    // This is the item used to summon a boss, in this case the modded Minion Boss from Example Mod. For vanilla boss summons, see comments in SetStaticDefaults
    public class TheRock : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 18;
			Item.maxStack = 1;
			Item.value = 100;
			Item.rare = ItemRarityID.Master;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return !NPC.AnyNPCs(ModContent.NPCType<RockBoss>());
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				// If the player using the item is the client
				// (explicitly excluded serverside here)
				SoundEngine.PlaySound(SoundID.Lavafall, player.position);

				int type = ModContent.NPCType<RockBoss>();

				if (Main.netMode != NetmodeID.MultiplayerClient) {
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else {
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CoreOfValhalla>(666666)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}