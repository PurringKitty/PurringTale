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
using Terraria.Graphics;
using Microsoft.CodeAnalysis;

namespace PurringTale.CatBoss
{
    public class Bolt : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.AmethystBolt}";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 270;
            Projectile.light = 1;
            Projectile.scale = 1;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        private ref float timer => ref Projectile.ai[0];
        private int target;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(target);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            target = reader.Read();
        }
        public override void AI()
        {
            Projectile.alpha = (int)Math.Clamp(Projectile.alpha - timer * 3, 0, 255);

            Projectile.velocity = Projectile.velocity.RotatedBy(0.005f);
            timer++;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (!target.HasBuff(ModContent.BuffType<Consumed>()))
                target.AddBuff(ModContent.BuffType<Consumed>(), 300);
            else
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Consumed>())] = 289;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Black;
            Draw(Projectile);
            return false;
        }
        private static VertexStrip _vertexStrip = new VertexStrip();

        public void Draw(Projectile proj)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        private Color StripColors(float progressOnStrip)
        {
            return Color.Black;
        }

        private float StripWidth(float progressOnStrip)
        {
            return MathHelper.Lerp(0f, 64f, MathF.Sqrt(progressOnStrip));
        }
    }
}