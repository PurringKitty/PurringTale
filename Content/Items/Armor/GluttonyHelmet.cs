using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GluttonyHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GluttonyBreastplate>() && legs.type == ModContent.ItemType<GluttonyLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.statLifeMax2 += 20;
            player.statDefense += 5;
            player.AddBuff(ModContent.BuffType<GluttonyBuff>(), 0);
        }
    }
}