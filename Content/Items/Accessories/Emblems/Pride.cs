using PurringTale.Common.Players;
using PurringTale.Content.Buffs;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Emblems;


public class Pride : ModItem
{
    public static readonly int PrideBoost = 10;
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 26;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Melee) += PrideBoost / 100f;

        player.AddBuff(BuffID.Warmth, 0);

    }
}