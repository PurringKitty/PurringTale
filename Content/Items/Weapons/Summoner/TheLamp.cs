using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Buffs;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles.MinionProjectiles;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Tiles.Furniture.Crafters;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class TheLamp : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 10;
        Item.knockBack = 2;
        Item.mana = 1;
        Item.width = 18;
        Item.height = 28;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Pink;
        Item.UseSound = SoundID.Item44;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.buffType = ModContent.BuffType<MothMinionBuff>();
        Item.shoot = ModContent.ProjectileType<MothMinionProjectile>();
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        player.AddBuff(Item.buffType, 2);

        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        return false;
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<ValhallaBar>(5);
        recipe.AddIngredient<CoreOfValhalla>(5);
        recipe.AddTile<ValhallaWorkbench>();
        recipe.Register();
    }

    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-2, 5);
        return offset;

    }
}