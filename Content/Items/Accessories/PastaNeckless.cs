using PurringTale.Common.Players;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories;

[AutoloadEquip(EquipType.Neck)]

public class PastaNeckless : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 32;
        Item.accessory = true;
        Item.sellPrice(gold: 2);
        Item.rare = ItemRarityID.Green;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetCritChance(DamageClass.Ranged) += 0.20f;
        player.GetAttackSpeed(DamageClass.Ranged) += 0.18f;
        player.GetDamage(DamageClass.Ranged) += 0.5f;
        player.GetArmorPenetration(DamageClass.Generic) += 0.05f;
        player.statLifeMax2 += 50;
        player.AddBuff(BuffID.Sunflower, 0);
    }
}