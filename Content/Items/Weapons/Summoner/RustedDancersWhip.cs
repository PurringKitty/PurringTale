using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.WhipProjectiles;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class RustedDancersWhip : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToWhip(projectileId: ModContent.ProjectileType<OldGodSlayerWhipProjectile>(), 23, 2, 4);
		Item.rare = ItemRarityID.Quest;
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