using PurringTale.Content.Items.MobLoot;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.NPCs.BossNPCs.ZeRock;

namespace PurringTale.Content.Items.Consumables.Summons
{
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
            Item.value = Item.sellPrice(gold: 1);
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
			if (player.whoAmI == Main.myPlayer)
			{
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
		public override void AddRecipes() 
		{
			CreateRecipe()
				.AddIngredient<CoreOfValhalla>(666)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}