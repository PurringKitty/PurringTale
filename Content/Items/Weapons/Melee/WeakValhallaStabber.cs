using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Projectiles.SwordProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class WeakValhallaStabber : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.knockBack = 1f;
        Item.useStyle = ItemUseStyleID.Rapier; // Makes the player do the proper arm motion
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.width = 32;
        Item.height = 32;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.autoReuse = false;
        Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
        Item.noMelee = true; // The projectile will do the damage and not the item

        Item.rare = ItemRarityID.White;
        Item.value = Item.sellPrice(0, 0, 0, 10);

        Item.shoot = ModContent.ProjectileType<ValhallaStabberProjectile>(); // The projectile is what makes a shortsword work
        Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<WeakValhallaBar>(13)
            .AddTile(TileID.Anvils)
            .Register();
    }
}