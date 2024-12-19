using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class FleshSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 100;
            Item.knockBack = 5f;
            Item.crit = 3;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item1;
        }
    }
}