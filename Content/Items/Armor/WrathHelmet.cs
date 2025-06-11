using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class WrathHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Red;
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WrathBreastplate>() && legs.type == ModContent.ItemType<WrathLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.autoJump = true;
            player.statLifeMax2 += 200;
            player.statManaMax2 -= 20000;
            player.AddBuff(ModContent.BuffType<WrathBuff>(), 0);
        }
    }
}