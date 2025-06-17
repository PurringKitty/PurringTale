using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Others;

public class HeartOfSlime : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 27;
        Item.height = 24;
        Item.accessory = true;
        Item.value = Item.sellPrice(silver: 15);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 75;
        player.statLifeMax2 += 75;
    }
}
