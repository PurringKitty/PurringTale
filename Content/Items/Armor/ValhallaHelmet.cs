using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ValhallaHelmet : ModItem
	{
        public static readonly int MeleeBonus = 10;
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ValhallaBreastplate>() && legs.type == ModContent.ItemType<ValhallaLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Melee) += MeleeBonus / 100f;
            player.statLifeMax2 += 20;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient<ValhallaBar>(20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

        }
    }
}

