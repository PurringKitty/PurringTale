using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Necklaces;

[AutoloadEquip(EquipType.Neck)]

public class TopHatDemonPendent : ModItem
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
        Item.rare = ItemRarityID.Master;
        Item.masterOnly = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.AddBuff(BuffID.IceBarrier, 0);

        player.AddBuff(BuffID.Campfire, 0);

        player.AddBuff(BuffID.HeartLamp, 0);

        player.AddBuff(BuffID.WellFed3, 0);

        player.AddBuff(BuffID.CatBast, 0);

        player.AddBuff(BuffID.RapidHealing, 0);

    }
}