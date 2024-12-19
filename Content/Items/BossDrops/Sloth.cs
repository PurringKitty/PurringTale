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


public class Sloth : ModItem
{
   
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.accessory = true;
        Item.sellPrice(gold: 75);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Magic) += 0.50f;

        player.GetCritChance(DamageClass.Magic) += 0.15f;

        player.GetAttackSpeed(DamageClass.Magic) += 0.10f;

        player.GetKnockback(DamageClass.Magic) += 0.10f;

        player.GetArmorPenetration(DamageClass.Magic) += 0.25f;

        player.AddBuff(BuffID.Featherfall, 0);

        player.AddBuff(ModContent.BuffType<SlothBuff>(), 0);

    }
}