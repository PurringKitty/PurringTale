using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GluttonyBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Green; 
            Item.defense = 9; 
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
            player.statLifeMax2 += 50;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GluttonusBar>(25)
                .AddIngredient<CoreOfGluttony>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}