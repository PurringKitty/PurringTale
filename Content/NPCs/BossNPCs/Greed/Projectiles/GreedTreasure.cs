using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.NPCs.BossNPCs.Greed.Projectiles
{
    public class GreedTreasure : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.GoldCoin}";

        private int[] treasureItems = {
            ItemID.GoldCoin, ItemID.SilverCoin, ItemID.Diamond, ItemID.Ruby, ItemID.Emerald,
            ItemID.GoldOre, ItemID.SilverOre, ItemID.GoldBar, ItemID.SilverBar,
            ItemID.GoldShortsword, ItemID.GoldBroadsword, ItemID.GoldBow,
            ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves
        };

        private int currentItemType;
        private int spriteChangeTimer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.light = 0.6f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1.1f;

            currentItemType = treasureItems[Main.rand.Next(treasureItems.Length)];
        }

        public override void AI()
        {
            spriteChangeTimer++;
            if (spriteChangeTimer >= 20)
            {
                currentItemType = treasureItems[Main.rand.Next(treasureItems.Length)];
                spriteChangeTimer = 0;
            }

            Projectile.rotation += 0.2f;

            Projectile.velocity.Y += 0.2f;
            if (Projectile.velocity.Y > 18f)
                Projectile.velocity.Y = 18f;

            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame);
                dust.velocity = Projectile.velocity * 0.3f;
                dust.noGravity = true;
                dust.scale = 0.9f;
                dust.color = Color.Gold;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (currentItemType <= 0 || currentItemType >= ItemLoader.ItemCount)
            {
                currentItemType = ItemID.GoldCoin;
            }

            Texture2D texture = null;
            try
            {
                texture = TextureAssets.Item[currentItemType].Value;
            }
            catch
            {
                texture = TextureAssets.Item[ItemID.GoldCoin].Value;
            }

            if (texture == null)
            {
                texture = TextureAssets.Projectile[Projectile.type].Value;
                if (texture == null)
                    return false;
            }

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            Color glowColor = Color.Lerp(Color.Gold, Color.Orange, 0.5f) * 0.4f;
            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = Vector2.One.RotatedBy(MathHelper.TwoPi / 3 * i) * 3f;
                Main.spriteBatch.Draw(texture, drawPos + offset, null, glowColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.position);

            if (Main.rand.NextBool(3))
            {
                SoundEngine.PlaySound(SoundID.Coins, Projectile.position);
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(2))
            {
                SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.position);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Coins, Projectile.position);
            }

            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = Vector2.One.RotatedBy(MathHelper.TwoPi / 12 * i) * Main.rand.NextFloat(2f, 6f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, velocity);
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.color = Color.Gold;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SoundEngine.PlaySound(SoundID.CoinPickup, Projectile.position);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.GoldCoin);
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }
        }
    }
}