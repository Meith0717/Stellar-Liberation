using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem.Projectiles
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

        public override void Update(Vector2 startPosition, GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            base.Update(startPosition, gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            Color c = TextureColor;
            for (int i = 1; i <= 6; i++)
            {
                TextureManager.Instance.DrawLine(mStartPosition, Position, c, 4 * i, TextureDepth);
                c = Color.Multiply(c, 0.3f);
            }
        }
    }
}

