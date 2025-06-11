using PurringTale.Content.Projectiles.SwordProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class LanceOfLust : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 40;
        Item.knockBack = 2f;
        Item.useStyle = ItemUseStyleID.Rapier;
        Item.useAnimation = 17;
        Item.useTime = 17;
        Item.width = 80;
        Item.height = 80;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(copper: 50);
        Item.shoot = ModContent.ProjectileType<LanceOfLustProjectile>();
        Item.shootSpeed = 5.0f;
    }
}