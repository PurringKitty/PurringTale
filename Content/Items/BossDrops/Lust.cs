using PurringTale.Common.Players;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.BossDrops;


public class Lust : ModItem
{
     public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.accessory = true;
        Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Summon) += 0.15f;

        player.GetCritChance(DamageClass.Summon) += 0.10f;

        player.GetAttackSpeed(DamageClass.Summon) += 0.10f;

        player.GetKnockback(DamageClass.Summon) += 0.10f;

        player.GetArmorPenetration(DamageClass.Summon) += 0.25f;

        player.AddBuff(BuffID.Lovestruck, 0);

        player.AddBuff(ModContent.BuffType<LustBuff>(), 0);

    }
}