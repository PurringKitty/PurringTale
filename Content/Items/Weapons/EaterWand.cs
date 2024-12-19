using Microsoft.Xna.Framework;
using PurringTale.Content;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons;

public class EaterWand : ModItem
{    
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.width = 58;
            Item.height = 60;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = true;
            Item.knockBack = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.CursedFlameFriendly;
            Item.mana = 10;
            Item.shootSpeed = 20f;
        }


        public override Vector2? HoldoutOffset()
        {
            Vector2 offset = new(-5,0);
            return offset;

        }
    }
