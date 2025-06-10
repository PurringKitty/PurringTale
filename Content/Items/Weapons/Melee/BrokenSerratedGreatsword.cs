using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons.Melee;

public class BrokenSerratedGreatsword : ModItem
{
     public override void SetDefaults()
     {
     Item.damage = 20;
     Item.DamageType = DamageClass.Melee;
     Item.width = 42;
     Item.height = 54;
     Item.useTime = 11;
     Item.useAnimation = 11;
     Item.useStyle = ItemUseStyleID.Swing;
     Item.useTurn = true;
     Item.knockBack = 10;
     Item.value = Item.sellPrice(silver: 50);
     Item.rare = ItemRarityID.Quest;
     Item.UseSound = SoundID.Item1;
     Item.autoReuse = true;
     }
}
