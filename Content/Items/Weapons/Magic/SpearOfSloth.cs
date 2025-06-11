using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace PurringTale.Content.Items.Weapons.Magic;

public class SpearOfSloth : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 130;
        Item.DamageType = DamageClass.Magic;
        Item.width = 35;
        Item.height = 37;
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.Swing;
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
    public override void SetStaticDefaults()
    {
        ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
    }
    public override bool? UseItem(Player player)
    {
        if (!Main.dedServ && Item.UseSound.HasValue)
        {
            SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
        }

        return null;
    }
}
