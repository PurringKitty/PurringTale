using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Buffs;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.Weapons
{
    public class FleshStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.knockBack = 2f;
            Item.mana = 15;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<FleshMinionBuff>();
            Item.shoot = ModContent.ProjectileType<FleshMinionProj>();
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




        public override Vector2? HoldoutOffset()
        {
            Vector2 offset = new(-5, -1);
            return offset;

        }
    }
}