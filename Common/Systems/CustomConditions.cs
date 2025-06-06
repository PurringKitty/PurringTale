using PurringTale.Common.Systems;
using Terraria;

namespace PurringTale.Common.Systems;
public static class CustomConditions
{
	public static Condition DownedEnvy = new("Mods.PurringTale.CustomConditions.DownedEnvy", () => DownedBossSystem.downedEnvy);
	public static Condition DownedGluttony = new("Mods.PurringTale.CustomConditions.DownedGluttony", () => DownedBossSystem.downedGluttony);
	public static Condition DownedGreed = new("Mods.PurringTale.CustomConditions.DownedGreed", () => DownedBossSystem.downedGreed);
	public static Condition DownedLust = new("Mods.PurringTale.CustomConditions.DownedLust", () => DownedBossSystem.downedLust);
	public static Condition DownedPride = new("Mods.PurringTale.CustomConditions.DownedPride", () => DownedBossSystem.downedPride);
	public static Condition DownedSloth = new("Mods.PurringTale.CustomConditions.DownedSloth", () => DownedBossSystem.downedSloth);
	public static Condition DownedWrath = new("Mods.PurringTale.CustomConditions.DownedWrath", () => DownedBossSystem.downedWrath);
	public static Condition DownedTopHat = new("Mods.PurringTale.CustomConditions.DownedTopHat", () => DownedBossSystem.downedTopHat);
	public static Condition DownedRock = new("Mods.PurringTale.CustomConditions.DownedRock", () => DownedBossSystem.downedRock);
}
