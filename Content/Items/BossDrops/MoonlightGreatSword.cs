using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Projectiles;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.BossDrops;

public class MoonlightGreatSword : ModItem
{
        public override void SetDefaults()
        {
        Item.damage = 800;
        Item.DamageType = DamageClass.Melee;
        Item.width = 58;
        Item.height = 64;
        Item.useTime = 19;
        Item.useAnimation = 19;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.master = true;
        Item.masterOnly = true;
        Item.shoot = ModContent.ProjectileType<MoonlightBlade2>();
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
