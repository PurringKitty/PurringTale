using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.CatBoss
{
	public class Debugger : ModItem
	{
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 2000000000;
            Item.knockBack = 0;
            Item.crit = -9;
            Item.value = Item.buyPrice(copper: 1);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item1;
        }
    }
}


