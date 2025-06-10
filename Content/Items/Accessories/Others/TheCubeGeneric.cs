using PurringTale.Common.Players;
using PurringTale.Content.DamageClasses;
using PurringTale.Content.Items.Placeables.Bars;
using PurringTale.Content.Items.Placeables.Furniture;
using PurringTale.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Accessories.Others
{ 
    [AutoloadEquip(EquipType.Back)]


    public class TheCubeGeneric : ModItem
    {
 public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(platinum: 50);
            Item.accessory = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar,100);
            recipe.AddIngredient<TopiumBar>(99999);
            recipe.AddIngredient<WeakValhallaBar>(99999);
            recipe.AddTile<Tiles.Furniture.ValhallaWorkbench>();
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 1.75f;
            player.GetCritChance(DamageClass.Generic) += 0.50f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.25f;
            player.GetArmorPenetration(DamageClass.Generic) += 0.50f;
            player.AddBuff(BuffID.Sharpened, 0);
            player.statManaMax2 += 200;
            player.statLifeMax2 += 200;
        }
    }
}