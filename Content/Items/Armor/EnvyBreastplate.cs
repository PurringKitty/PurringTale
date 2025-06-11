using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class EnvyBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Green; 
            Item.defense = 3; 
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 40;
            player.statLifeMax2 += 20;
        }
    }
}