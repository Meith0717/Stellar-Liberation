// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        private readonly SpaceShip mSpaceShip;

        private double mBias1;
        private double mBias2;
        private double mBias3;

        public CombatBehavior(SpaceShip spaceShip) 
        {
            mSpaceShip = spaceShip;
            mBias1 = .4f + ExtendetRandom.Random.NextSingle() * .2f;
            mBias2 = .2f + ExtendetRandom.Random.NextSingle() * .2f;
            mBias3 = .3f + ExtendetRandom.Random.NextSingle() * .3f;
        }

        public override double GetScore()
        {
            if (mSpaceShip.SensorSystem.OpponentsInRannge.Count <= 0) return 0;

            mSpaceShip.SensorSystem.TryGetAimingShip(mSpaceShip.Position, out var aimingTarget);
            mSpaceShip.PhaserCannaons.AimShip(aimingTarget);

            var shielHhullScore = mSpaceShip.DefenseSystem.ShieldPercentage * mBias1 + mSpaceShip.DefenseSystem.HullPercentage * (1 - mBias1);
            var targetShielHhullScore = aimingTarget.DefenseSystem.ShieldPercentage * mBias2 + aimingTarget.DefenseSystem.HullPercentage * (1 - mBias2);

            var score = shielHhullScore * mBias3 + (1 - targetShielHhullScore) * mBias3;
            return score;
        }

        public override void Execute()
        {
            mSpaceShip.SublightDrive.SetVelocity(.1f);
            mSpaceShip.SublightDrive.FollowSpaceShip(mSpaceShip.PhaserCannaons.AimingShip);
            mSpaceShip.PhaserCannaons.Fire();
        }

        public override void Recet()
        {
            mSpaceShip.PhaserCannaons.RecetAimingShip();
            mSpaceShip.PhaserCannaons.StopFire();
            mBias1 = .4f + ExtendetRandom.Random.NextSingle() * .2f;
            mBias2 = .2f + ExtendetRandom.Random.NextSingle() * .2f;
            mBias3 = .3f + ExtendetRandom.Random.NextSingle() * .3f;
        }
    }
}
