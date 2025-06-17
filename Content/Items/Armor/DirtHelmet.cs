using PurringTale.Content.Tiles.Furniture.Crafters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DirtHelmet : ModItem
    {
        public static readonly int Dirt = 10;
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(copper: 5);
            Item.rare = ItemRarityID.Gray;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DirtBreastplate>() && legs.type == ModContent.ItemType<DirtLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.statLifeMax2 += Dirt;
            player.statManaMax2 += Dirt;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 15)
                .AddTile<ValhallaWorkbench>()
                .Register();
        }
    }
}