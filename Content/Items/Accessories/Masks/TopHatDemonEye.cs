using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Masks;


public class TopHatDemonEye : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
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

        player.GetDamage(DamageClass.Generic) += 1.25f;

        player.GetCritChance(DamageClass.Generic) += 0.50f;

        player.GetAttackSpeed(DamageClass.Generic) += 0.25f;

        player.GetKnockback(DamageClass.Generic) += 0.75f;

        player.GetArmorPenetration(DamageClass.Generic) += 0.75f;

    }
}