using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class EnvyHelmet : ModItem
    {
        
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Green;
            Item.defense = 3;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EnvyBreastplate>() && legs.type == ModContent.ItemType<EnvyLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.AddBuff(ModContent.BuffType<EnvyBuff>(), 0);
        }
    }
}