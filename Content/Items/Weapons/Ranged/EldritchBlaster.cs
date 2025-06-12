using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

	public class EldritchBlaster : ModItem
	{
    public override void SetDefaults() {
        Item.damage = 800;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 54;
        Item.height = 30;
        Item.useTime = 6;
        Item.useAnimation = 6;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item12;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.TerrarianBeam;
        Item.useAmmo = AmmoID.Bullet;
        Item.shootSpeed = 40f;
        Item.master = true;
        Item.masterOnly = true;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(2, 3);
        return offset;
    }
}
