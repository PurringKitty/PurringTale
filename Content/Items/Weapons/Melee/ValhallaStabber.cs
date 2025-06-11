using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Projectiles.SwordProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class ValhallaStabber : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 18;
        Item.knockBack = 1f;
        Item.useStyle = ItemUseStyleID.Rapier; 
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.width = 32;
        Item.height = 32;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.autoReuse = false;
        Item.noUseGraphic = true; 
        Item.noMelee = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 0, 0, 10);

        Item.shoot = ModContent.ProjectileType<ValhallaStabberProjectile>(); 
        Item.shootSpeed = 2.0f; 
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<ValhallaBar>(15)
            .AddTile(TileID.Anvils)
            .Register();
    }
}