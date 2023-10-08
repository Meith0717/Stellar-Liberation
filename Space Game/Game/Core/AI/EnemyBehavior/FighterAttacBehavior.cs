using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.Utility;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class FighterAttacBehavior : Behavior
    {
        private readonly int mAttacDistance;

        private float mDistanceToTarget;
        private bool mReorienting;
        private Vector2? mReorientingPosition;

        public FighterAttacBehavior(int attacDistance) 
        { 
            mAttacDistance = attacDistance;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            if (spaceShip.Target is null) return 0;
            mDistanceToTarget = Vector2.Distance(spaceShip.Target.Position, spaceShip.Position);
            if (mDistanceToTarget > mAttacDistance) return 0;
            return 0.5;
        }

        public override void Execute(List<GameObject> environment, SpaceShip spaceShip)
        {

            switch (mReorienting)
            {
                case true:
                    mReorienting = mDistanceToTarget < mAttacDistance * 0.9f;

                    if (mReorientingPosition is not null) break;
                    Utility.Utility.Random.NextUnitVector(out var unitVector);
                    var attacDistance = Utility.Utility.Random.Next(mAttacDistance / 2, mAttacDistance);
                    mReorientingPosition = spaceShip.Position + (unitVector * attacDistance * 2);
                    spaceShip.SublightEngine.SetTarget(mReorientingPosition);
                    break;
                case false:
                    mReorientingPosition = null;
                    mReorienting = mDistanceToTarget < 25000;
                    spaceShip.SublightEngine.SetTarget(spaceShip.Target.Position); ;
                    break;
            }

            var angleBetweenTarget = Geometry.AngleBetweenVectors(spaceShip.Position, spaceShip.Target.Position);
            var angleToTarget = MathF.Abs(Geometry.AngleRadDelta(spaceShip.Rotation, angleBetweenTarget));
            if (angleToTarget < 5) spaceShip.WeaponSystem.Fire(spaceShip, spaceShip.Target.Position);
        }

    }
}
