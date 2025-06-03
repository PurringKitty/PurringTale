using Microsoft.Xna.Framework;
using PurringTale.Content.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PurringTale.Content.Items.MobLoot;
using PurringTale.Content.Items.Placeables;

namespace PurringTale.Content.Items
{
    public class GlitchTorch : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;

            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch;
            ItemID.Sets.SingleUseInGamepad[Type] = true;
            ItemID.Sets.Torches[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToTorch(ModContent.TileType<Tiles.GlitchTorch>(), 0, false);
            Item.value = 50;
            Item.rare = ItemRarityID.Green;
        }

        public override void HoldItem(Player player)
        {

            if (player.wet)
            {
                return;
            }

            if (Main.rand.NextBool(player.itemAnimation > 0 ? 7 : 30))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(player.itemLocation.X + (player.direction == -1 ? -16f : 6f), player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<GlitchSolution>(), 0f, 0f, 100);
                if (!Main.rand.NextBool(3))
                {
                    dust.noGravity = true;
                }

                dust.velocity *= 0.3f;
                dust.velocity.Y -= 1.5f;
                dust.position = player.RotatedRelativePoint(dust.position);
            }


            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);

            Lighting.AddLight(position, 1f, 1f, 1f);
        }

        public override void PostUpdate()
        {

            if (!Item.wet)
            {
                Lighting.AddLight(Item.Center, 1f, 1f, 1f);
            }
        }

        public override void AddRecipes()
        {
            Recipe gt = CreateRecipe(3);
            gt.AddIngredient(ModContent.ItemType<CoreOfGlitch>());
            gt.AddIngredient(ItemID.Wood, 2);
            gt.AddTile(TileID.WorkBenches);
            gt.Register();
        }

    }
}