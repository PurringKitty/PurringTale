using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Buffs
{
	public class WhipTagDebuff : ModBuff
	{
		public static readonly int TagDamage = 5;

		public override void SetStaticDefaults() {
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

	public class WhipTaggDebuff : ModBuff
	{
		public static readonly int TagDamagePercent = 30;
		public static readonly float TagDamageMultiplier = TagDamagePercent / 100f;

		public override void SetStaticDefaults() {
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

	public class WhipTagDebuffNPC : GlobalNPC
	{
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
				return;


			// SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
			var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
			if (npc.HasBuff<WhipTagDebuff>()) {
				// Apply a flat bonus to every hit
				modifiers.FlatBonusDamage += WhipTagDebuff.TagDamage * projTagMultiplier;
			}

			// if you have a lot of buffs in your mod, it might be faster to loop over the NPC.buffType and buffTime arrays once, and track the buffs you find, rather than calling HasBuff many times
			if (npc.HasBuff<WhipTaggDebuff>()) {
				// Apply the scaling bonus to the next hit, and then remove the buff, like the vanilla firecracker
				modifiers.ScalingBonusDamage += WhipTaggDebuff.TagDamageMultiplier * projTagMultiplier;
				npc.RequestBuffRemoval(ModContent.BuffType<WhipTaggDebuff>());
			}
		}
	}
}
