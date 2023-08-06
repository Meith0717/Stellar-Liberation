using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.WeaponSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.Weapons
{
    internal class Laser : Weapon
    {
        private SpaceShip mOriginObj;
        private GameObject mTargetObj;
        private Vector2 mVariance;
        private float mVelocity;

        public Laser(SpaceShip originObj, GameObject targetObj) 
            : base(Vector2.Zero, ContentRegistry.pixle.Name, 5, 0, 10000)
        {
            mOriginObj = originObj;
            mVelocity = 10f;
            mTargetObj = targetObj;

            var angleToTarget = Geometry.AngleBetweenVectors(originObj.Position, mTargetObj.Position + mVariance);
            Position = Geometry.GetPointOnCircle(originObj.Position, originObj.BoundedBox.Radius + 10, angleToTarget);
            Rotation = Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget)));

            var variance = (int)(Math.Min(targetObj.Width, targetObj.Height) * 0.5f);
            mVariance = Utility.GetRandomVector2(-variance, variance, -variance, variance);
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
            Color c = Color.MonoGameOrange;
            for (int i = 1; i <= 6; i++)
            {
                TextureManager.Instance.DrawLine(mOriginObj.Position, Position, c, 4 * i, 0);
                c = Color.Multiply(c, 0.25f);
            }
        }
    }
}

