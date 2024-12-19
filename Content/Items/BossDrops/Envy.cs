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

public class Envy : ModItem
{
    public override void SetDefaults()

    {
        Item.width = 40;
        Item.height = 40;
        Item.accessory = true;
        Item.sellPrice(silver: 5);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Magic) += 0.15f;

        player.GetCritChance(DamageClass.Magic) += 0.07f;

        player.GetAttackSpeed(DamageClass.Magic) += 0.05f;

        player.GetKnockback(DamageClass.Magic) += 0.25f;

        player.GetArmorPenetration(DamageClass.Magic) += 0.10f;

        player.AddBuff(BuffID.Spelunker, 0);

        player.AddBuff(ModContent.BuffType<EnvyBuff>(), 0);

    }
}