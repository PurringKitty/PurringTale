using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using PurringTale.Content.Projectiles.SwordProjectiles;

namespace PurringTale.Content.Items.Weapons.Melee;

public class MoonlightGreatSword : ModItem
{
        public override void SetDefaults()
        {
        Item.damage = 1000;
        Item.DamageType = DamageClass.Melee;
        Item.width = 58;
        Item.height = 64;
        Item.useTime = 18;
        Item.useAnimation = 18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.knockBack = 100;
        Item.crit = 100;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Master;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.master = true;
        Item.masterOnly = true;
        Item.shoot = ModContent.ProjectileType<MoonlightBlade>();
        Item.noMelee = true;
        Item.shootsEveryUse = true;
        Item.autoReuse = true;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        float adjustedItemScale = player.GetAdjustedItemScale(Item);
        Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

}
