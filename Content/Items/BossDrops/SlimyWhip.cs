using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.BossDrops
{
    public class SlimyWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(ModContent.ProjectileType<SlimyWhipProjectile>(), 15, 2, 10);
			Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 5);
            Item.channel = true;
			Item.autoReuse = true;
		}

		// Makes the whip receive melee prefixes
		public override bool MeleePrefix()
		{
			return true;
        }
    }
}