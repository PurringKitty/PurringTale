using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.WhipProjectiles;

namespace PurringTale.Content.Items.Weapons.Summoner;

    public class SlimyWhip : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToWhip(ModContent.ProjectileType<SlimyWhipProjectile>(), 20, 2, 4);
		Item.rare = ItemRarityID.Blue;
		Item.shootSpeed = 4;
        Item.value = Item.sellPrice(silver: 50);
        Item.channel = true;
		Item.autoReuse = true;
	}

	// Makes the whip receive melee prefixes
	public override bool MeleePrefix()
	{
		return true;
    }
}