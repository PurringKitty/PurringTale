using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GreedHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GreedBreastplate>() && legs.type == ModContent.ItemType<GreedLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.statLifeMax2 -= 30;
            player.statManaMax2 -= 30;
            player.AddBuff(ModContent.BuffType<GreedDeBuff>(), 0);
        }
    }
}