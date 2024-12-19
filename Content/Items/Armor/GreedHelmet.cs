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
    public class GreedHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 9;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GreedBreastplate>() && legs.type == ModContent.ItemType<GreedLeggings>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GreedyBar>(20)
                .AddIngredient<CoreOfGreed>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}