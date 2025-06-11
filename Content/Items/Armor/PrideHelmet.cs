using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PrideHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 15;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrideBreastplate>() && legs.type == ModContent.ItemType<PrideLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.statLifeMax2 += 50;
            player.autoReuseAllWeapons = true;
            player.AddBuff(ModContent.BuffType<PrideBuff>(), 0);
        }
    }
}