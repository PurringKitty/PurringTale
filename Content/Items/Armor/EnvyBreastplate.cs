using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Body)]
    public class EnvyBreastplate : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Green; 
            Item.defense = 7; 
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 50;
            player.statLifeMax2 += 20;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<EnvyousBar>(25)
                .AddIngredient<CoreOfEnvy>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}