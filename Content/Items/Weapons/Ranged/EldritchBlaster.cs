using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Ranged;

	public class EldritchBlaster : ModItem
	{
    public override void SetDefaults() {
        Item.damage = 800;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 58;
        Item.height = 38;
        Item.useTime = 4;
        Item.useAnimation = 4;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTurn = false;
        Item.knockBack = 10;
        Item.value = Item.sellPrice(gold: 100);
        Item.rare = ItemRarityID.Expert;
        Item.UseSound = SoundID.Item12;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.TerrarianBeam;
        Item.useAmmo = AmmoID.Bullet;
        Item.shootSpeed = 100f;
        Item.master = true;
        Item.masterOnly = true;
    }
}
