using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.Utility;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior : Behavior
    {
        private readonly float mVelocity;
        private readonly float mAttacDistance;

        private float mDistanceToTarget;
        private bool mReorienting;

        public AttacBehavior(int velocity, int attacDistance) 
        { 
            mVelocity = Utility.Utility.Random.Next(velocity - 5, velocity + 5);
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

            spaceShip.Velocity = mVelocity;
            switch (mReorienting)
            {
                case true:
                    mReorienting = mDistanceToTarget < mAttacDistance * 0.9f;
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Target.Position, spaceShip.Position, 0.1f);
                    break;
                case false:
                    mReorienting = mDistanceToTarget < 25000;
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, spaceShip.Target.Position, 0.1f);
                    break;
            }

            var angleBetweenTarget = Geometry.AngleBetweenVectors(spaceShip.Position, spaceShip.Target.Position);
            var angleToTarget = MathF.Abs(Geometry.AngleRadDelta(spaceShip.Rotation, angleBetweenTarget));
            if (angleToTarget < 5) spaceShip.WeaponSystem.Fire(spaceShip);
        }

    }
}
