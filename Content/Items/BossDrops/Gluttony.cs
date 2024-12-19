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


public class Gluttony : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.defense = 20;
        Item.accessory = true;
        Item.sellPrice(silver: 50);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Ranged) += 0.15f;

        player.GetCritChance(DamageClass.Ranged) += 0.15f;

        player.GetAttackSpeed(DamageClass.Ranged) += 0.07f;

        player.GetKnockback(DamageClass.Ranged) += 0.25f;

        player.GetArmorPenetration(DamageClass.Ranged) += 0.10f;

        player.AddBuff(BuffID.WellFed, 0);

        player.AddBuff(ModContent.BuffType<GluttonyBuff>(), 0);

    }
}