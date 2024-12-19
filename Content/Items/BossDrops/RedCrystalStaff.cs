using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.MobLoot;

namespace PurringTale.Content.Items.BossDrops;

public class RedCrystalStaff : ModItem
{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.PurringTale.hjson file.

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("CrystalSlimeProj").Type;
            Item.mana = 2;
            Item.shootSpeed = 6f;
        }
        public override Vector2? HoldoutOffset()
        {
            Vector2 offset = new(-20, -5);
            return offset;

        }
    }
