using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using MathNet.Numerics.Distributions;
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

        public WeaponSystem(List<Vector2> relativePositions, int coolDown)
        {
            mRelativePositions = relativePositions;
            mProjectiles = new List<Projectile>();
            mMaxCoolDown = coolDown;
        }

        public virtual void Fire(SpaceShip spaceShip, Vector2 target)
        { 
            if (mCooldown < mMaxCoolDown) return;
            foreach (var relPos in mRelativePositions)
            {                
                var position = GetPosition(spaceShip.Position, relPos, spaceShip.Rotation);
                var fireRotation = Geometry.AngleBetweenVectors(position, target);
                mProjectiles.Add(new Projectile(spaceShip, position, fireRotation, 5, 1, 200));
            }
            mCooldown = 0;
            SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);
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

        public void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
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
                projectile.Update(gameTime, inputState, sceneLayer);
            }

            foreach (var projectile in deleteList)
            {
                projectile.RemoveFromSpatialHashing(sceneLayer);
                mProjectiles.Remove(projectile);
            }
        }

    }
}
