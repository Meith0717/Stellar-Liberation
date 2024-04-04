// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        private readonly Spaceship mSpaceship;
        private bool mReposition;
        private double mBias1;

        public CombatBehavior(Spaceship spaceShip)
        {
            mSpaceship = spaceShip;
            mBias1 = .7f + ExtendetRandom.Random.NextSingle() * .2f;
        }

        public override double GetScore()
        {
            if (mSpaceship.SensorSystem.OpponentsInRannge.Count <= 0) return 0;

            var shielHhullScore = mSpaceship.DefenseSystem.ShieldPercentage * mBias1 + mSpaceship.DefenseSystem.HullPercentage * (1 - mBias1);

            var score = shielHhullScore;
            return score;
        }

        public override void Execute()
        {
            mSpaceship.SensorSystem.TryGetAimingShip(mSpaceship.Position, out var target);
            var distance = Vector2.Distance(target.Position, mSpaceship.Position);

            var dotProduct = Vector2.Dot(mSpaceship.MovingDirection, target.MovingDirection);
            var velocity = 0f;
            switch (mReposition)
            {
                case false:
                    mSpaceship.SublightDrive.MoveInDirection(Vector2.Normalize(target.Position - mSpaceship.Position));
                    velocity = (dotProduct + 1) / 2;
                    if (distance <= mSpaceship.BoundedBox.Diameter * 5) mReposition = true;
                    break;
                case true:
                    mSpaceship.SublightDrive.MoveInDirection(-Vector2.Normalize(target.Position - mSpaceship.Position));
                    velocity = (-dotProduct + 1) / 2;
                    if (distance >= mSpaceship.BoundedBox.Diameter * 30) mReposition = false;
                    break;
            }
            mSpaceship.SublightDrive.SetVelocity(MathHelper.Clamp(velocity, 0.2f, 1f));
        }

        public override void Recet()
        {
            mBias1 = .4f + ExtendetRandom.Random.NextSingle() * .2f;
            mSpaceship.SublightDrive.SetVelocity(0);
        }
    }
}
