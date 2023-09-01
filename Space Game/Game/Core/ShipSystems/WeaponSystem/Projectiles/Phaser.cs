using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem.Projectiles
{
    public class Phaser : Projectile
    {
        internal Phaser(Vector2 position, SpaceShip targetObj, Color color, int shieldDamage, int hullDamage) 
            : base(position, targetObj, ContentRegistry.pixle.Name, 1, shieldDamage, hullDamage, 25f)
        {
            TextureColor = color;
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position + mVariance);
            Rotation = Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget)));
        }

        public override void Update(Vector2 startPosition, GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            RemoveFromSpatialHashing(sceneLayer);
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            base.Update(startPosition, gameTime, inputState, sceneLayer);
            AddToSpatialHashing(sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            Color c = TextureColor;
            for (int i = 1; i <= 6; i++)
            {
                TextureManager.Instance.DrawLine(mStartPosition, Position, c, 4 * i, TextureDepth);
                c = Color.Multiply(c, 0.3f);
            }
        }
    }
}

