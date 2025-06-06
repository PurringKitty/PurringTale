using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Weapons
{
	public class GolemWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(projectileId: ModContent.ProjectileType<GolemWhipProj>(), 90, 2, 5);
			Item.rare = ItemRarityID.Master;
            Item.shootSpeed = 4;
            Item.value = Item.sellPrice(silver: 50);
            Item.channel = true;
            Item.autoReuse = true;
        }

		public override bool MeleePrefix()
		{
			return true;
		}


	}
}