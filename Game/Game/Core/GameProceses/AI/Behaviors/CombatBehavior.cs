// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        private readonly SpaceShip mSpaceShip;
        private bool mReposition;
        private double mBias1;

        public CombatBehavior(SpaceShip spaceShip) 
        {
            mSpaceShip = spaceShip;
            mBias1 = .7f + ExtendetRandom.Random.NextSingle() * .2f;
        }

        public override double GetScore()
        {
            if (mSpaceShip.SensorSystem.OpponentsInRannge.Count <= 0) return 0;

            var shielHhullScore = mSpaceShip.DefenseSystem.ShieldPercentage * mBias1 + mSpaceShip.DefenseSystem.HullPercentage * (1 - mBias1);

            var score = shielHhullScore;
            return score;
        }

        public override void Execute()
        {
            mSpaceShip.SensorSystem.TryGetAimingShip(mSpaceShip.Position, out var target);
            var distance = Vector2.Distance(target.Position, mSpaceShip.Position);

            var dotProduct = Vector2.Dot(mSpaceShip.MovingDirection, target.MovingDirection);
            var velocity = 0f;
            switch (mReposition)
            {
                case false:
                    mSpaceShip.PhaserCannaons.Fire();
                    mSpaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(target.Position - mSpaceShip.Position));
                    velocity = (dotProduct + 1) / 2;
                    if (distance <= mSpaceShip.BoundedBox.Diameter * 5) mReposition = true;
                    break;
                case true:
                    mSpaceShip.PhaserCannaons.StopFire();
                    mSpaceShip.SublightDrive.MoveInDirection(-Vector2.Normalize(target.Position - mSpaceShip.Position));
                    velocity = (- dotProduct + 1) / 2;
                    if (distance >= mSpaceShip.BoundedBox.Diameter * 30) mReposition = false;
                    break;
            }
            mSpaceShip.SublightDrive.SetVelocity(MathHelper.Clamp(velocity, 0.2f, 1f));
        }

        public override void Recet()
        {
            mSpaceShip.PhaserCannaons.StopFire();
            mBias1 = .4f + ExtendetRandom.Random.NextSingle() * .2f;
        }
    }
}
