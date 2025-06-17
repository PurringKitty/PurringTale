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
using static Humanizer.In;

namespace PurringTale.CatBoss
{
    /// ai[0] timer
    /// ai[1] attack type
    /// ai[2] boss index
    /// ai[3] attack style
	public class BossClone : ModProjectile
    {

        public override string Texture => "PurringTale/CatBoss/TopHatShadow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 3;
            Main.projFrames[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 54;
            Projectile.height = 30;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0;
            Projectile.damage = 0;
        }
        private ref float timer => ref Projectile.ai[0];
        private int[] laserIndex = new int[4];
        public int attackStyle;
        public Vector2 centerPoint;
        public Vector2 aDirection;
        public int entranceDelay;
        private Vector2 off;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(aDirection);
            writer.Write(entranceDelay);
            writer.Write(attackStyle);
            writer.WriteVector2(centerPoint);
            writer.WriteVector2(off);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            aDirection = reader.ReadVector2();
            entranceDelay = reader.Read();
            attackStyle = reader.Read();
            centerPoint = reader.ReadVector2();
            off = reader.ReadVector2();
        }
        public override void OnSpawn(IEntitySource source)
        {
            NPC owner = Main.npc[(int)Projectile.ai[2]];
            Player target = Main.player[owner.target];
            off = target.Center - Projectile.Center;
        }
        public override bool PreAI()
        {
            if (--entranceDelay <= 0)
            {
                return true;
            }
            else
            {
                NPC owner = Main.npc[(int)Projectile.ai[2]];
                Player target = Main.player[owner.target];

                Projectile.Center = target.Center + off;
                centerPoint = target.Center;
            }
            return false;
        }

        public override void AI()
        {
            Projectile.Opacity = 1;
            AttackType AttackType = (AttackType)Projectile.ai[1];
            NPC owner = Main.npc[(int)Projectile.ai[2]];
            if (!owner.active)
            {
                Projectile.timeLeft = 2;
            }
            timer++;
            Projectile.alpha = (int)Math.Clamp(Projectile.alpha - timer * 3, 0, 255);
            Projectile.frame = 0;

            staffAttack(owner);
            switch (AttackType)
            {
                case AttackType.Book:  
                    break;
                case AttackType.Sword:
                    break;
                default:
                    break;
            }
            
        }
        private void bookAttack()
        {

        }
        private void swordAttack()
        {

        }
        private void staffAttack(NPC owner)
        {
            switch (attackStyle)
            {
                case 1:
                    int i = 30;


                    break;
                case 3:

                    int i2 = 5;

                    if (timer < i2)
                    {
                        Projectile.velocity = Projectile.Center.DirectionTo(centerPoint) * 15;
                    }
                    if (timer > i2 && timer < i2 + 10)
                    {
                        Projectile.velocity *= 0.25f;
                    }
                    if (timer == i2 + 10)
                    {
                        laserIndex[0] = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.Center.DirectionTo(centerPoint), ModContent.ProjectileType<BossLaser>(), 100, 5, -1, 0, Projectile.whoAmI + Main.maxNPCs, 20);
                        Main.projectile[laserIndex[0]].timeLeft = 80;
                    }
                    if (timer == i2 + 90)
                    {
                        Main.projectile[laserIndex[0]].Kill();
                    }
                    if (timer == i2 + 94)
                    {
                        Projectile.velocity = Projectile.Center.DirectionTo(centerPoint) * -15;
                    }
                    if (timer > i2+94)
                    {
                        Projectile.velocity *= 1.01f;
                    }
                    if (timer == i2 + 140)
                    {
                        Projectile.Kill();
                    }
                    break;
            }
            Projectile.spriteDirection = Projectile.Center.DirectionTo(centerPoint).X > 0 ? 1 : -1;

        }
        private void gunAttack()
        {

        }
        private void whipAttack()
        {

        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float tl = (float)Projectile.oldPos.Length;
            Main.instance.LoadProjectile(Type);
            Texture2D t = TextureAssets.Projectile[Type].Value;
            Rectangle source = new Rectangle(0, 0, t.Width, (int)(36));
            SpriteEffects spriteEffects = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (float i = 0; i < tl; i += (float)(tl / 3))
            {
                float percent = i / tl;
                Vector2 dpos = Projectile.oldPos[(int)i] - Main.screenPosition + t.center() * Projectile.scale - Vector2.UnitY * 12;
                Main.spriteBatch.Draw(t, dpos, source, Color.Purple * (1 - percent) * Projectile.Opacity, Projectile.rotation, t.center(), Projectile.scale, spriteEffects, 0);
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
    }
}

