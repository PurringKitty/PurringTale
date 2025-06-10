using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using PurringTale.Content.Projectiles.SpearProjectiles;

namespace PurringTale.Content.Items.Weapons.Melee;

public class PlantSpear : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 55;
        Item.DamageType = DamageClass.Melee;
        Item.width = 35;
        Item.height = 37;
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.useTurn = true;
        Item.knockBack = 4.5f;
        Item.value = Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item71;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
        Item.shoot = ModContent.ProjectileType<PlantSpearProj>();
        Item.shootSpeed = 13f;
    }
    public override void SetStaticDefaults()
    {
        ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
        ItemID.Sets.Spears[Item.type] = true;
    }
    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[Item.shoot] < 1;
    }

    public override bool? UseItem(Player player)
    {
        if (!Main.dedServ && Item.UseSound.HasValue)
        {
            SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
        }

        return null;
    }
    public override Vector2? HoldoutOffset()
    {
        Vector2 offset = new(100, 2);
        return offset;

    }
}
