﻿using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Emblems;

public class Envy : ModItem
{
    public static readonly int EnvyBoost = 5;
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()

    {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.Lime;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Magic) += EnvyBoost / 100f;

        player.waterWalk = true;

    }
}