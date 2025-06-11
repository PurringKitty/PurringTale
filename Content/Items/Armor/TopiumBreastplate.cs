using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class TopiumBreastplate : ModItem
    	{
		public static int MaxManaIncrease = 100;
        public static int MaxLifeIncrease = 100;
        public override void SetDefaults() 
        {
			Item.width = 28;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(gold: 100);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 42; 
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.Bleeding] = true;
            player.statManaMax2 += MaxManaIncrease;
            player.statLifeMax2 += MaxLifeIncrease;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddIngredient<TopiumBar>(25);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
