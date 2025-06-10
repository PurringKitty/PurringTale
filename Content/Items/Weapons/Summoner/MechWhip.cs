using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.WhipProjectiles;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class MechWhip : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToWhip(projectileId: ModContent.ProjectileType<MechWhipProj>(), 100, 2, 4);
		Item.rare = ItemRarityID.Expert;
		Item.shootSpeed = 6;
        Item.value = Item.sellPrice(silver: 50);
        Item.channel = true;
		Item.autoReuse = true;
	}

	public override bool MeleePrefix()
	{
		return true;
	}
}