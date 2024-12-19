using PurringTale.Common.Players;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.BossDrops;

public class HeartOfSlime : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 27;
        Item.height = 24;
        Item.accessory = true;
        Item.sellPrice(copper: 50);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetAttackSpeed(DamageClass.Generic) += 0.10f;
        player.GetCritChance(DamageClass.Generic) += 0.25f;
        player.GetDamage(DamageClass.Generic) += 0.15f;
        player.buffImmune[BuffID.Bleeding] = true;
        player.buffImmune[BuffID.Burning] = true;
        player.buffImmune[BuffID.Confused] = true;
        player.statManaMax2 += 100;
        player.statLifeMax2 += 100;
    }
}
