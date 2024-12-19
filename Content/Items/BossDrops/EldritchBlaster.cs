using PurringTale.Content.Projectiles;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
#pragma warning disable CS0105 // Using directive appeared previously in this namespace
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.MobLoot;
#pragma warning restore CS0105 // Using directive appeared previously in this namespace

namespace PurringTale.Content.Items.BossDrops;

	public class EldritchBlaster : ModItem
	{
    public override void SetDefaults() {
        Item.damage = 800;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 58;
        Item.height = 38;
        Item.useTime = 4;
        Item.useAnimation = 4;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item12;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.TerrarianBeam;
        Item.useAmmo = AmmoID.Bullet;
        Item.shootSpeed = 100f;
        Item.master = true;
        Item.masterOnly = true;

    }


    public override bool AltFunctionUse(Player player)
    {
        return base.AltFunctionUse(player);
    }

}
