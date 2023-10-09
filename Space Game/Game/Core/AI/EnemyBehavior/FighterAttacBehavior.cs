using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.Utility;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using CelestialOdyssey.Game.Core.ShipSystems;

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

        public override double GetPriority(SensorArray environment, SpaceShip spaceShip)
        {
            if (spaceShip.Target is null) return 0;
            mDistanceToTarget = Vector2.Distance(spaceShip.Target.Position, spaceShip.Position);
            if (mDistanceToTarget > mAttacDistance) return 0;
            return 0.5;
        }

        public override void Execute(SensorArray environment, SpaceShip spaceShip)
        {
            switch (mReorienting)
            {
                case true:
                    // Check for breaking reorientation
                    if (mDistanceToTarget > mAttacDistance * 0.9f)
                    {
                        mReorienting = false;
                        // Set course to attac target
                        spaceShip.SublightEngine.SetTarget(spaceShip, spaceShip.Target.Position);
                    }

                    // Check if reorientation Position has been reached 
                    if (!spaceShip.SublightEngine.IsTargetReached(spaceShip, mReorientingPosition)) break;
                    // Get new reorientation Position
                    GetReorientingPosition(spaceShip);
                    // Set course to new reorientation Position
                    spaceShip.SublightEngine.SetTarget(spaceShip, mReorientingPosition);
                    break;

                case false:
                    mReorientingPosition = null;

                    // Check if a reorientation is needet 
                    if (mDistanceToTarget < 50000)
                    {
                        mReorienting = true;
                        // Get reorientation Position
                        GetReorientingPosition(spaceShip);
                        // Set course to reorientation Position
                        spaceShip.SublightEngine.SetTarget(spaceShip, mReorientingPosition);
                        break;
                    }

                    // Set course to attac target
                    spaceShip.SublightEngine.SetTarget(spaceShip, spaceShip.Target.Position);
                    break;
            }

            // Fire on Target
            var angleBetweenTarget = Geometry.AngleBetweenVectors(spaceShip.Position, spaceShip.Target.Position);
            var angleToTarget = MathF.Abs(Geometry.AngleRadDelta(spaceShip.Rotation, angleBetweenTarget));
            if (angleToTarget < 5) spaceShip.WeaponSystem.Fire(spaceShip.ActualPlanetSystem.ProjectileManager, spaceShip, spaceShip.Target.Position);
        }

        private void GetReorientingPosition(SpaceShip spaceShip)
        {
            Utility.Utility.Random.NextUnitVector(out var unitVector);
            var attacDistance = Utility.Utility.Random.Next(mAttacDistance / 2, mAttacDistance * 2);
            mReorientingPosition = spaceShip.Position + (unitVector * attacDistance * 2);
        }

    }
}
