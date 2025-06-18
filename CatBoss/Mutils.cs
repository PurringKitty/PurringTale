using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

public class MCameraModifiers : ModSystem
{
    public void Shake(Vector2 start, float strength, int frames)
    {
        PunchCameraModifier modifier = new PunchCameraModifier(start, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), strength, 6f, (int)frames, 1000f, FullName);
        Main.instance.CameraModifiers.Add(modifier);
    }
}
public static class ModdingusUtils
{
    public static void TrackClosestPlayer(this NPC npc, float speed, float inertia, float range)
    {
        Player target = FindClosestPlayer(npc, range);

        if (target != null)
        {
            Vector2 direction = npc.DirectionTo(target.Center) * speed;

            npc.velocity.X = (npc.velocity.X * (inertia - 1) + direction.X) / inertia;
            npc.velocity.Y = (npc.velocity.Y * (inertia - 1) + direction.Y) / inertia;
        }
    }
    public static void TrackClosestPlayer(this Projectile Projectile, float speed, float inertia, float range)
    {
        Player target = FindClosestPlayer(Projectile, range);

        if (target != null)
        {
            Vector2 direction = Projectile.DirectionTo(target.Center) * speed;

            Projectile.velocity.X = (Projectile.velocity.X * (inertia - 1) + direction.X) / inertia;
            Projectile.velocity.Y = (Projectile.velocity.Y * (inertia - 1) + direction.Y) / inertia;
        }
    }
    public static Player FindClosestPlayer(this Entity entity, float maxDetectDistance)
    {
        Player closestNPC = null;


        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;


        for (int k = 0; k < Main.maxPlayers; k++)
        {
            Player target = Main.player[k];
            float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, entity.Center);
            if (sqrDistanceToTarget < sqrMaxDetectDistance)
            {
                sqrMaxDetectDistance = sqrDistanceToTarget;
                closestNPC = target;

            }
        }
        return closestNPC;
    }
    public static Vector2 Normalized(this Vector2 vector)
    {
        if (vector.Length() > 0)
            return vector / vector.Length();
        else
            return vector;
    }
    public static Vector2 randomVector()
    {
        return Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi));
    }
    /// <summary>
    /// returns the center of the current frame as coords on the texture
    /// </summary>
    public static Vector2 origin(this NPC npc, int offY = 0, int offX = 0)
    {
        Main.instance.LoadNPC(npc.whoAmI);
        Texture2D texture = TextureAssets.Npc[npc.whoAmI].Value;
        return new Vector2(texture.Width / 2 + offX, npc.height / 2 + offY + npc.frame.Y);
    }
    public static Rectangle source(this Texture2D tex)
    {
        return new Rectangle(0, 0, tex.Width, tex.Height);
    }
    public static Vector2 center(this Texture2D t)
    {
        return new Vector2(t.Width / 2, t.Height / 2);
    }
    public static T RandomFromEnum<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(Main.rand.Next(v.Length));
    }
    public static Vector2 random(this Rectangle rect)
    {
        return new Vector2(Main.rand.NextFloat(rect.X, rect.X + rect.Width), Main.rand.NextFloat(rect.Y, rect.Y + rect.Height));
    }
    public static Vector2 randomCorner()
    {
        return new Vector2(PoN1(), PoN1());
    }
    public static Vector2 randomSide()
    {
        if (Main.rand.NextBool())
        {
            return new Vector2(PoN1(), 0);
        }
        else
        {
            return new Vector2(0, PoN1());
        }
    }
    public static int PoN1()
    {
        return Main.rand.NextBool() ? -1 : 1;
    }
    public static void DrawTriangle(SpriteBatch spriteBatch, Triangle t)
    {
        Texture2D tex = TextureAssets.MagicPixel.Value;
        spriteBatch.Draw(tex, new Rectangle((int)(t.A.X - Main.screenPosition.X), (int)(t.A.Y - Main.screenPosition.Y), (int)t.AB, 1), tex.source(), Color.Red, t.BAC / 2, Vector2.Zero, SpriteEffects.None, 0);
        spriteBatch.Draw(tex, new Rectangle((int)(t.A.X - Main.screenPosition.X), (int)(t.A.Y - Main.screenPosition.Y), (int)t.AC, 1), tex.source(), Color.Red, -t.BAC / 2, Vector2.Zero, SpriteEffects.None, 0);
        spriteBatch.Draw(tex, new Rectangle((int)(t.C.X - Main.screenPosition.X), (int)(t.C.Y - Main.screenPosition.Y), (int)t.BC, 1), tex.source(), Color.Red, t.ACB / 2, Vector2.Zero, SpriteEffects.None, 0);
    }
    /// <summary>
    /// Looks for a Player in a cone towards the projectile's velocity. Ignores hostility (de)buffs (eg. Putrid Scent, Beetle Armor)
    /// </summary>
    /// <param name="projectile"></param>
    /// <param name="MaxAngle"></param>
    /// <returns>The index of the closest player who meets requirements or -1 if no player is found</returns>
    public static int FindPlayerInLineOfSight(this Projectile projectile, float MaxAngle, float MaxRange)
    {
        Vector2 b = projectile.Center + Vector2.Normalize(projectile.velocity).RotatedBy(MaxAngle) * MaxRange;
        Vector2 c = projectile.Center + Vector2.Normalize(projectile.velocity).RotatedBy(-MaxAngle) * MaxRange;
        Triangle t = new Triangle(projectile.Center, b, c);

        int temp = -1;
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            var target = Main.player[i];

            if (t.Contains(target.Center) && target.Distance(projectile.Center) < target.Distance(projectile.Center))
            {
                temp = i;
            }
        }
        return temp;
    }
}
public struct Triangle : IEquatable<Triangle>
{
    public Vector2 A;
    public Vector2 B;
    public Vector2 C;

    readonly public float AB;
    readonly public float BC;
    readonly public float AC;

    readonly public float ABC;
    readonly public float BAC;
    readonly public float ACB;

    readonly public float area;

    public Triangle(Vector2 a, Vector2 b, Vector2 c)
    {
        A = a;
        B = b;
        C = c;

        AB = A.Distance(B);
        BC = B.Distance(C);
        AC = A.Distance(C);

        ABC = (float)Math.Acos(AB * AB + AC * AC - BC * BC) / 2 * AC * AB;
        BAC = (float)Math.Acos(AB * AB + BC * BC - AC * AC) / 2 * BC * AB;
        ACB = (float)Math.Acos(AC * AC + BC * BC - AB * AB) / 2 * AC * BC;

        area = (float)Math.Abs((A.X * (B.Y - C.Y) +
                                     B.X * (C.Y - A.Y) +
                                     C.X * (A.Y - B.Y)) / 2f);
    }
    public float Area(Vector2 _A, Vector2 _B, Vector2 _C)
    {
        return Math.Abs((_A.X * (_B.Y - _C.Y) +
                     _B.X * (_C.Y - _A.Y) +
                     _C.X * (_A.Y - _B.Y)) / 2f);
    }
    public override bool Equals(object obj)
    {
        if (obj is Triangle)
        {
            return Equals((Triangle)obj);
        }
        return false;
    }
    public bool Contains(Vector2 p)
    {
        double a1 = Area(p, B, C);

        double a2 = Area(A, p, C);

        double a3 = Area(A, B, p);

        return (area == a1 + a2 + a3);
    }
    public bool Similar(Triangle other)
    {
        return other.ABC == ABC && other.BAC == BAC && other.ACB == ACB;
    }
    public override string ToString()
    {
        return $"A: {A}, B: {B}, C: {C}";
    }

    public bool Equals(Triangle other)
    {
        return other.A == A && other.B == B && other.C == C && other.AB == AB && other.BC == BC && other.AC == AC;
    }

    public static bool operator ==(Triangle left, Triangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Triangle left, Triangle right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

