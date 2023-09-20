using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.AI.EnemyBehavior
{
    public class AttacBehavior : Behavior
    {
        private SpaceShip mTarget = null;
        private float mDistanceToTarget;
        private float mAttacVelocity;
        private float mReorientVelocity;
        private bool mReorienting;

        public AttacBehavior() 
        { 
            mAttacVelocity = Utility.Utility.Random.Next(30, 40);
            mReorientVelocity = Utility.Utility.Random.Next(50, 60);
            mReorienting = false;
        }

        public override double GetPriority(List<GameObject> environment, SpaceShip spaceShip)
        {
            GetTarget(environment);
            if (mTarget is null) return 0;
            mDistanceToTarget = Vector2.Distance(mTarget.Position, spaceShip.Position);

            return mDistanceToTarget switch
            {
                < 250000 => (mReorienting ? 0.5 : 0.7),
                _ => 0
            };
        }

        private void GetTarget(List<GameObject> environment)
        {
            var targets = environment.OfType<SpaceShip>().ToList();
            if (targets.Count <= 0) { return; }
            mTarget = targets.First();
        }


        public override void Execute(SpaceShip spaceShip)
        {

            switch (mReorienting)
            {
                case true:
                    spaceShip.Velocity = mReorientVelocity;
                    mReorienting = mDistanceToTarget < 200000;
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, mTarget.Position, spaceShip.Position, 0.05f);
                    break;
                case false:
                    spaceShip.Velocity = mAttacVelocity;
                    mReorienting = mDistanceToTarget < 25000;
                    spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, mTarget.Position, 0.05f);
                    break;
            }

            var angleBetweenTarget = Geometry.AngleBetweenVectors(spaceShip.Position, mTarget.Position);
            var angleToTarget = MathF.Abs(Geometry.AngleRadDelta(spaceShip.Rotation, angleBetweenTarget));
            if (angleToTarget < 10) spaceShip.WeaponSystem.Fire(spaceShip);
        }

    }
}
