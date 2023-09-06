using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class Projectile : GameObject
    {
        public float LiveTime = 5000;
        private Vector2 mDirection;
        private float mVelocity;
        private int mShieldDamage;
        private int mHullDamage;

        internal Projectile(Vector2 startPosition, float rotation, int shieldDamage, int hullDamage, float velocity)
            : base(startPosition, ContentRegistry.projectile, 10, 20)
        {
            mDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            mVelocity = velocity;
            mHullDamage = hullDamage;
            mShieldDamage = shieldDamage;
        }

        public void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer, SpaceShip origin)
        {
            base.Update(gameTime, inputState, sceneLayer);
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            RemoveFromSpatialHashing(sceneLayer);
            Position += mDirection * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            AddToSpatialHashing(sceneLayer);
            CheckForHit(origin, sceneLayer);
        }

        private void CheckForHit(SpaceShip origin, SceneLayer sceneLayer)
        {
            var items = sceneLayer.GetSortedObjectsInRadius<SpaceShip>(Position, 10000);
            items.Remove(origin);
            foreach (var item in items)
            {
                if (!item.BoundedBox.Intersects(BoundedBox)) return;
                item.DoDamage(mShieldDamage, mHullDamage);
                LiveTime = 0;
            }
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
