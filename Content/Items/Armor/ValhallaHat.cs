using PurringTale.Content.Items.Placeables.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ValhallaHat : ModItem
	{
        public static readonly int RangedBonus = 10; 
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }
        public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
            Item.wornArmor = true;
            Item.value = Item.sellPrice(silver: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 8;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ValhallaBreastplate>() && legs.type == ModContent.ItemType<ValhallaLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += RangedBonus / 100f;
            player.GetAttackSpeed(DamageClass.Ranged) += RangedBonus / 100f;
            player.moveSpeed += RangedBonus / 100f;
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

