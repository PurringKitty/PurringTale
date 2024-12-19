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
using Terraria.DataStructures;
using PurringTale.Content.Items.Consumables;
using Terraria.GameContent.ItemDropRules;
#pragma warning restore CS0105 // Using directive appeared previously in this namespace

namespace PurringTale.Content.Items.BossDrops;

	public class SlimeK47 : ModItem
	{
		// You can use a vanilla texture for your item by using the format: "Terraria/Item_<Item ID>".

   

        public override void SetDefaults() {
        Item.damage = 10;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 56;
        Item.height = 16;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 1;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.NPCHit1;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.useAmmo = AmmoID.Bullet;
        Item.shootSpeed = 100f;
        

    }


    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {

        Vector2 offset = new(velocity.X * 3, velocity.Y * 3);
        position += offset;
        if (type == ProjectileID.Bullet)
        {
            type = Mod.Find<ModProjectile>("GelBulletProjectile").Type;
        }
        return true;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(-6, 2);
        return offset;

    }
}

	