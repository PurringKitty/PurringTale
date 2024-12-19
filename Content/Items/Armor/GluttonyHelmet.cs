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
    public class GluttonyHelmet : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<GluttonyBreastplate>() && legs.type == ModContent.ItemType<GluttonyLeggings>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GluttonusBar>(20)
                .AddIngredient<CoreOfGluttony>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}