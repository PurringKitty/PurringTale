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


public class Wrath : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.accessory = true;
        Item.sellPrice(platinum: 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Melee) += 0.50f;

        player.GetCritChance(DamageClass.Melee) += 0.50f;

        player.GetAttackSpeed(DamageClass.Melee) += 0.15f;

        player.GetKnockback(DamageClass.Melee) += 0.25f;

        player.GetArmorPenetration(DamageClass.Melee) += 0.30f;

        player.AddBuff(BuffID.Rage, 0);

        player.AddBuff(ModContent.BuffType<WrathBuff>(), 0);

    }
}