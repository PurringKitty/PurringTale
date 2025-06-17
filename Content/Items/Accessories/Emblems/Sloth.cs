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


public class Sloth : ModItem
{
    public static readonly int SlothBoost = 10;
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
        Item.rare = ItemRarityID.LightPurple;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Magic) += SlothBoost / 100f;

        player.AddBuff(BuffID.Featherfall, 0);


    }
}