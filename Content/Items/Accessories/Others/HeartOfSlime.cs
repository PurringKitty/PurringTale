using PurringTale.Common.Players;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Others;

public class HeartOfSlime : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 27;
        Item.height = 24;
        Item.accessory = true;
        Item.sellPrice(silver: 15);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
        player.GetCritChance(DamageClass.Generic) += 0.25f;
        player.GetDamage(DamageClass.Generic) += 0.15f;
        player.statManaMax2 += 75;
        player.statLifeMax2 += 75;
    }
}
