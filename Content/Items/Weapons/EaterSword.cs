using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Items.Weapons
{
    public class EaterSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 32;
            Item.knockBack = 5f;
            Item.crit = 5;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Master;
            Item.UseSound = SoundID.Item1;
        }
    }
}