using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GreedBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Yellow; 
            Item.defense = 10;
        }
    }
}