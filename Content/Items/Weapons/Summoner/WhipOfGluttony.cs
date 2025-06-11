using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Projectiles.WhipProjectiles;

namespace PurringTale.Content.Items.Weapons.Summoner;
public class WhipOfGluttony : ModItem
{
	public override void SetStaticDefaults()
	{
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToWhip(projectileId: ModContent.ProjectileType<GluttonyWhipProjectile>(), 25, 2, 4);
		Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(copper: 50);
        Item.shootSpeed = 4;
        Item.channel = true;
        Item.autoReuse = true;
    }
    public override bool MeleePrefix()
	{
		return true;
	}
}