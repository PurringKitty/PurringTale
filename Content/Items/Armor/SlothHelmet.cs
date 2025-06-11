using PurringTale.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SlothHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 17;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SlothBreastplate>() && legs.type == ModContent.ItemType<SlothLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.statManaMax2 += 150;
            player.statLifeMax2 -= 150;
            player.AddBuff(ModContent.BuffType<SlothBuff>(), 0);
        }
    }
}