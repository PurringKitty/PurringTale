using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PrideBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Purple; 
            Item.defense = 15;
        }
    }
}