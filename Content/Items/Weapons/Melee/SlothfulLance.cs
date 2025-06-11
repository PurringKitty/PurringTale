using PurringTale.Content.Projectiles.SwordProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class SlothfulLance : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 130;
        Item.knockBack = 1f;
        Item.useStyle = ItemUseStyleID.Rapier;
        Item.useAnimation = 14;
        Item.useTime = 14;
        Item.width = 76;
        Item.height = 76;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true; 
        Item.rare = ItemRarityID.Purple;
        Item.value = Item.sellPrice(silver: 50);
        Item.shoot = ModContent.ProjectileType<SlothfulLanceProjectile>();
        Item.shootSpeed = 5.0f;
    }
}