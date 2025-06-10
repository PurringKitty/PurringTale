using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Magic;

public class PlantWand : ModItem
{
     public override void SetDefaults()
     {
        Item.damage = 55;
        Item.DamageType = DamageClass.Magic;
        Item.width = 42;
        Item.height = 16;
        Item.useTime = 13;
        Item.useAnimation = 13;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = true;
        Item.knockBack = 2;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item20;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.VilethornBase;
        Item.mana = 20;
        Item.shootSpeed = 20f;
     }
}
