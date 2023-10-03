using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core
{
    public static class Configs
    {
        public const int SensorArrayCoolDown = 100;
        public static class Enemy
        {
            public const int PatrollVelocity = 50;
            public const int FollowVelocity = 50;
            public const int FleeVelovity = 50;
            public const int AttacVelocity = 50;
            public const int AttackDistance = 250000;
            public const int ScanDistance = 2000000;
        }
    }
}
