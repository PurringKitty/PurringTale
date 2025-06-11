using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GluttonyBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Green; 
            Item.defense = 6; 
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
            player.statLifeMax2 += 50;
        }
    }
}