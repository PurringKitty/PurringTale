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
        Item.width = 16;
        Item.height = 14;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 75);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.statDefense += 20;

        player.lifeRegen = 10;

    }
}