using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;

namespace PurringTale.CatBoss
{
	public class BossLaser : ModProjectile
	{
        /// ai[0] timer
        /// ai[1] owner
        /// ai[2] draw offset;
        public override string Texture => "PurringTale/CatBoss/Assets/2";

        const float BEAMLEN = 1350;
        float warningTimer = 10;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 360;
            Projectile.light = 1;
            Projectile.scale = 0.1f;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        private ref float timer => ref Projectile.ai[0];

        private Vector2 spawnPos;
        public override void OnSpawn(IEntitySource source)
        {
            spawnPos = Projectile.Center;
            ModContent.GetInstance<MCameraModifiers>().Shake(Projectile.Center, 5f, 30);
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(spawnPos);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            spawnPos = reader.ReadVector2();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (warningTimer > 0)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopRight(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.Normalize(Projectile.velocity) * (BEAMLEN*Projectile.scale));
        }
        public override bool PreAI() => --warningTimer <= 0;

        public override void AI()
        {
            if (Projectile.scale < 1)
            {
                Projectile.scale += 0.05f;
            }
            Vector2 ownerCenter;
            if (Projectile.ai[1] < Main.maxNPCs)
            {
                ownerCenter = Main.npc[(int)Projectile.ai[1]].Center;
            }
            else
            {
                ownerCenter = Main.projectile[(int)(Projectile.ai[1] - Main.maxNPCs)].Center;
            }
            if (Projectile.timeLeft < 6)
            {
                Projectile.scale -= 0.2f;
            }
            timer++;
            Projectile.Center = ownerCenter;
            Projectile.alpha = (int)Math.Clamp(Projectile.alpha - timer * 3, 0, 255);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (warningTimer > 0)
            {
                Vector2 pos = Projectile.Center - Main.screenPosition;
                Rectangle dest = new Rectangle(((int)pos.X), ((int)pos.Y), ((int)(BEAMLEN + 20)), 2);
                Texture2D t = TextureAssets.MagicPixel.Value;
                Main.spriteBatch.Draw(t, dest, t.source(), Color.Red, Projectile.velocity.ToRotation(), t.center(), SpriteEffects.None, 0); 
            } else {
                Vector2 dir = Vector2.Normalize(Projectile.velocity);
                Vector2 dpos = Projectile.Center - Main.screenPosition;
                Texture2D texture = ModContent.Request<Texture2D>("PurringTale/CatBoss/Assets/Beam").Value;
                Rectangle start = new(0, 0, 26, 22);
                Rectangle mid = new(0, 24, 23, 30);
                Rectangle end = new(0, 54, 26, 22);

                int i = (int)(start.Height * 2 * Projectile.scale + Projectile.ai[2]);

                Main.spriteBatch.Draw(texture, dpos + (dir * i), start, lightColor, dir.ToRotation() - MathHelper.PiOver2, texture.center(), Projectile.scale, SpriteEffects.None, 1);

                for (i += (int)(start.Height * Projectile.scale); i < BEAMLEN * Projectile.scale; i += (int)(mid.Height * Projectile.scale))
                {
                    Main.spriteBatch.Draw(texture, dpos + (dir * i), mid, lightColor, dir.ToRotation() - MathHelper.PiOver2, texture.center(), Projectile.scale, SpriteEffects.None, 1);
                }
                Main.spriteBatch.Draw(texture, dpos + (dir * (i - 2)), end, lightColor, dir.ToRotation() - MathHelper.PiOver2, texture.center(), Projectile.scale, SpriteEffects.None, 1);
            }
            return false;
        }
    }
    public static class uwu
    {
        public static int getDirection(this NPC npc)
        {
            return npc.targetRect.Center.X < npc.Center.X ? -1 : 1;
        }
    }
}

