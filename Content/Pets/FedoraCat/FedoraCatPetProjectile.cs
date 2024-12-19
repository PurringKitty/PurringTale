using Microsoft.Xna.Framework;
using PurringTale.Content.Pets.Boots;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PurringTale.Content.Pets.FedoraCat
{
	public class FedoraCatPetProjectile : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 6)
           .WithOffset(-10, -20f)
           .WithSpriteDirection(-1)
           .WithCode(DelegateMethods.CharacterPreview.BerniePet);
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;
            Projectile.timeLeft *= 5;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 0.8f;
            Projectile.tileCollide = false;
            Projectile.CloneDefaults(ProjectileID.Wisp);
            AIType = ProjectileID.Wisp;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.dead && player.HasBuff(ModContent.BuffType<FedoraCatPetBuff>()))
            {
                Projectile.timeLeft = 2;
            }
        } 
    }
}