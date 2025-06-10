using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.WhipProjectiles;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class BrainWhip : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToWhip(projectileId: ModContent.ProjectileType<BrainWhipProj>(), 26, 2, 4);
		Item.rare = ItemRarityID.LightRed;
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