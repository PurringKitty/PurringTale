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
    public class LustHelmet : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 11;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<LustBreastplate>() && legs.type == ModContent.ItemType<LustLeggings>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<LusterBar>(20)
                .AddIngredient<CoreOfLust>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}