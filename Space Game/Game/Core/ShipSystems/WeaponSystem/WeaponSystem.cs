using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class WeaponSystem
    {
        private List<Vector2> mRelativePositions;
        private List<Projectile> mProjectiles;
        private int mMaxCoolDown;
        private int mCooldown;

        public WeaponSystem(List<Vector2> relativePositions)
        {
            mRelativePositions = relativePositions;
            mProjectiles = new List<Projectile>();
            mMaxCoolDown = 100;
        }

        public virtual void Fire(SpaceShip spaceShip)
        { 
            if (mCooldown < mMaxCoolDown) return;
            foreach (var relPos in mRelativePositions)
            {                
                var position = GetPosition(spaceShip.Position, relPos, spaceShip.Rotation);
                mProjectiles.Add(new Projectile(position, spaceShip.Rotation, 5, 5, 100));
            }
            mCooldown = 0;
            SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);
        }

        public void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer, SpaceShip origin)
        {
            mCooldown += gameTime.ElapsedGameTime.Milliseconds;

            List<Projectile> deleteList = new();
            foreach (var projectile in mProjectiles)
            {
                if (projectile.LiveTime <= 0)
                {
                    deleteList.Add(projectile);
                    continue;
                }
                projectile.Update(gameTime, inputState, sceneLayer, origin);
            }

            foreach (var projectile in deleteList)
            {
                projectile.RemoveFromSpatialHashing(sceneLayer);
                mProjectiles.Remove(projectile);
            }
        }

        private static Vector2 GetPosition(Vector2 origin, Vector2 relativePosition, float rotation)
        {
            float cosTheta = (float)Math.Cos(rotation);
            float sinTheta = (float)Math.Sin(rotation);
            Vector2 rotatedVector = new Vector2(
                relativePosition.X * cosTheta - relativePosition.Y * sinTheta,
                relativePosition.X * sinTheta + relativePosition.Y * cosTheta
            );
            return rotatedVector + origin;
        }
    }
}
