using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Buffs;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Projectiles.MinionProjectiles;
using PurringTale.Content.Items.Placeables.Bars;

namespace PurringTale.Content.Items.Weapons.Summoner;

public class GodSlayerSummoningBook : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 400;
        Item.knockBack = 20;
        Item.mana = 10;
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 50);
        Item.rare = ItemRarityID.Quest;
        Item.UseSound = SoundID.Item44;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.buffType = ModContent.BuffType<GodSlayerMinionBuff>();
        Item.shoot = ModContent.ProjectileType<GodSlayerMinionProjectile>();
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
        recipe.AddIngredient<RuinedGodSlayerSummoningBook>(1);
        recipe.AddIngredient<TopiumBar>(28);
        recipe.AddIngredient<CoreOfValhalla>(20);
        recipe.AddIngredient(ItemID.LunarBar, 100);
        recipe.AddTile(TileID.LunarCraftingStation);
        recipe.Register();
    }

    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-18, 5);
        return offset;

    }
}