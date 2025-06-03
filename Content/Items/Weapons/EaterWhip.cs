using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Weapons
{
	public class EaterWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.DefaultToWhip(projectileId: ModContent.ProjectileType<EaterWhipProj>(), 30, 2, 4);
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