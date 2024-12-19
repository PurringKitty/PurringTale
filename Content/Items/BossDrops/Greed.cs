using PurringTale.Common.Players;
using PurringTale.Content.Buffs;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.BossDrops;


public class Greed : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.accessory = true;
        Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Generic) += 0.15f;

        player.GetCritChance(DamageClass.Generic) += 0.15f;

        player.GetAttackSpeed(DamageClass.Generic) += 0.15f;

        player.GetKnockback(DamageClass.Generic) += 0.15f;

        player.GetArmorPenetration(DamageClass.Generic) += 0.15f;

        player.AddBuff(BuffID.Midas, 0);

        player.AddBuff(ModContent.BuffType<GreedBuff>(), 0);

    }
}