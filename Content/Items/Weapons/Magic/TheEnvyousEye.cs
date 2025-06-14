using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Magic;

public class TheEnvyousEye : ModItem
{
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Magic;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("EnvyEyeProj").Type;
            Item.mana = 5;
            Item.shootSpeed = 7f;
        }
        public override Vector2? HoldoutOffset()
        {
            Vector2 offset = new(-2, 5);
            return offset;
        }
}
