using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Magic;

public class BookOfSloth : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 130;
        Item.DamageType = DamageClass.Magic;
        Item.width = 14;
        Item.height = 15;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTurn = false;
        Item.knockBack = 6.5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Purple;
        Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.DeathSickle;
        Item.mana = 10;
        Item.shootSpeed = 10f;
    }
}