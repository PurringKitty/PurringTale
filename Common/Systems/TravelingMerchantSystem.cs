using PurringTale.Content.NPCs;
using PurringTale.Content.NPCs.TownNPCs;
using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PurringTale.Common.Systems;

public class TravelingMerchantSystem : ModSystem
{
	public override void PreUpdateWorld() {
		TopHatCat.UpdateTravelingMerchant();
	}

	public override void SaveWorldData(TagCompound tag) {
		if (TopHatCat.spawnTime != double.MaxValue) {
			tag["ExampleTravelingMerchantSpawnTime"] = TopHatCat.spawnTime;
		}
	}

	public override void LoadWorldData(TagCompound tag) {
		if (!tag.TryGet("ExampleTravelingMerchantSpawnTime", out TopHatCat.spawnTime)) {
			TopHatCat.spawnTime = double.MaxValue;
		}
	}
}