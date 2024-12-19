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
    public class WrathLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Red;
            Item.defense = 23;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.WeaponImbueFire, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WrathiorBar>(15)
                .AddIngredient<CoreOfWrath>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
