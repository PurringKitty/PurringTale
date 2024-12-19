using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Legs)]
    public class PrideLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Swiftness, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PridefulBar>(15)
                .AddIngredient<CoreOfPride>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
