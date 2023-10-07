using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using System.Net;
using static CelestialOdyssey.Game.Core.Configs;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    internal class Weapon : GameObject
    {
        private Vector2 mRelativePosition;
        private Color mWeaponColor;
        private int mShieldDamage;
        private int mHullDamage;

        public Weapon(Vector2 relativePosition, float textureScale, int textureDepth, Color color, int shieldDamage, int hullDamage)
        : base(Vector2.Zero, ContentRegistry.turette, textureScale, textureDepth)
        {
            mWeaponColor = color;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
            mRelativePosition = relativePosition;
        }

        public Projectile Fire(SpaceShip origin, Vector2 target)
        {
            Rotation = Geometry.AngleBetweenVectors(Position, target);
            var projectile = new Projectile(Position, Rotation, mWeaponColor, mShieldDamage, mHullDamage, origin);
            return projectile;
        }

        public void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene, float shipRotation, Vector2 position)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            Position = GetPosition(position, mRelativePosition, shipRotation); ;
            UpdateBoundBox();
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
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