using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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
            Item.defense = 19;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SlothBreastplate>() && legs.type == ModContent.ItemType<SlothLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.statManaMax2 += 140;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SlothyBar>(20)
                .AddIngredient<CoreOfSloth>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}