using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class LustHelmet : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 13;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<LustBreastplate>() && legs.type == ModContent.ItemType<LustLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.statManaMax2 += 20;
            player.maxMinions += 1; 
            player.AddBuff(ModContent.BuffType<LustBuff>(), 0);
        }
    }
}