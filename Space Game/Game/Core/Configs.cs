using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core
{
    public static class Configs
    {
        public const int SensorArrayCoolDown = 1000;

        public static class Fighter
        {
            public const int ScanRadius = 2000000;
            
        }


        public static class Enemy
        {
            public static Color WeaponColor = Color.Red;
            public const int InitialShieldDamage = 1;
            public const int InitialHullDamage = 1;
            public const int InitialCoolDown = 1000; // ms

            public const int PatrollVelocity = 50;
            public const int FollowVelocity = 50;
            public const int FleeVelovity = 50;
            public const int AttacVelocity = 50;
            public const int AttackDistance = 250000;
            public const int ScanDistance = 2000000;
        }

        public static class Player
        {
            public static Color WeaponColor = Color.LightBlue;
            public const int InitialShieldDamage = 1;
            public const int InitialHullDamage = 1;
            public const int InitialCoolDown = 500; // ms
        }

        public static class Projectile
        {
            public const int Velocity = 250;
            public const int LiveTime = 5000; // ms
        }

    }
}
