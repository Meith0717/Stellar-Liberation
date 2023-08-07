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
        private SpaceShip mOriginObj;

        internal Phaser(SpaceShip originObj, SpaceShip targetObj, Color color, int shieldDamage, int hullDamage) 
            : base(originObj.Position, targetObj, ContentRegistry.pixle.Name, 1, shieldDamage, hullDamage, 25f)
        {
            mOriginObj = originObj;
            TextureColor = color;
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position + mVariance);
            Position = Geometry.GetPointOnCircle(Position, BoundedBox.Radius + 10, angleToTarget);
            Rotation = Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget)));
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            Color c = TextureColor;
            for (int i = 1; i <= 6; i++)
            {
                TextureManager.Instance.DrawLine(mOriginObj.Position, Position, c, 4 * i, 0);
                c = Color.Multiply(c, 0.3f);
            }
        }
    }
}

