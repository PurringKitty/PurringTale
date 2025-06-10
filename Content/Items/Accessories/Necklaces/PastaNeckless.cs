using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Necklaces;

[AutoloadEquip(EquipType.Neck)]

public class PastaNeckless : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
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
        player.GetAttackSpeed(DamageClass.Ranged) += 0.15f;
        player.GetDamage(DamageClass.Ranged) += 0.5f;
        player.GetArmorPenetration(DamageClass.Generic) += 0.5f;
        player.statLifeMax2 += 50;
        player.AddBuff(BuffID.Sunflower, 0);
    }
}