using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Emblems;


public class Lust : ModItem
{
    public static readonly int LustBoost = 25;
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
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetDamage(DamageClass.Summon) += LustBoost / 100f;

        player.AddBuff(BuffID.Lovestruck, 0);

        player.maxMinions += 2;

        player.whipRangeMultiplier += 2;
    }
}