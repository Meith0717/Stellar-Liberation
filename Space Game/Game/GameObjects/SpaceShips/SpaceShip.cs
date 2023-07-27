using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.GameObjects.Weapons;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : InteractiveObject
    {
        public float Velocity { get; internal set; } = 0;
        internal Vector2 mMovingDirection;
        internal float mMaxVelocity = 5;


        private float mMaxShieldForce = 100;
        private float mShieldForce;
        private float mMaxHullForce = 100;
        private float mHullForce;

        public SpaceShip(Vector2 position, string textureId, float textureScale, int textureDepth)
            : base(position, textureId, textureScale, textureDepth)
        {
            mShieldForce = mMaxHullForce;
            mHullForce = mMaxHullForce;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {
            RemoveFromSpatialHashing(gameEngine);
            mMovingDirection += Geometry.CalculateDirectionVector(Rotation) * Velocity;
            Position = mMovingDirection * gameTime.ElapsedGameTime.Milliseconds;
            ChechForHit(gameEngine);
            base.Update(gameTime, inputState, gameEngine);
            AddToSpatialHashing(gameEngine);
        }

        internal void ChechForHit(GameEngine.GameEngine engine)
        {
            List<Projectile> projectiles = engine.GetObjectsInRadius<Projectile>(Position, 200);
            foreach (var item in projectiles)
            {
                if (!BoundedBox.Contains(item.Position)) continue;
                item.LiveTime = float.PositiveInfinity;
                DoDamage(1);
            }
        }

        private void DoDamage(int damage)
        {
            if (mShieldForce > 0)
            {
                mShieldForce -= damage;
                return;
            }
            mHullForce -= mHullForce > 0 ? damage : 0;
        }

        internal bool SpaceshipsInRadius(GameEngine.GameEngine gameEngine, out List<SpaceShip> spaceShips) 
        {
            spaceShips = gameEngine.GetObjectsInRadius<SpaceShip>(Position, 10000);
            spaceShips.Remove(this);
            if (spaceShips.Count == 0) return false;
            return true;
        }

        public double ShildLevel { get { return mShieldForce / mMaxShieldForce; } }
        public double HullLevel { get { return mHullForce / mMaxHullForce; } }
        public double SpeedLevel { get { return Velocity / mMaxVelocity; } }

    }
}
