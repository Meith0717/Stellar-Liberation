using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.WeaponSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.Weapons
{
    public enum ProjectileType
    {
        None
    }

    internal class Torpedo : Weapon
    {
        private float mVelocity = 1f;
        private GameObject mTargetObj;
        private Vector2 mVariance;

        public Torpedo(SpaceShip originObj, GameObject targetObj, float rotation, ProjectileType type)
            : base(Geometry.GetPointOnCircle(originObj.Position, originObj.BoundedBox.Radius + 10, originObj.Rotation - MathF.PI),  "projectile", 0.5f, 1, 10000)
        {
            mTargetObj = targetObj;
            Rotation = rotation;
            TextureColor = Color.MonoGameOrange;

            var variance = (int)(Math.Min(targetObj.Width, targetObj.Height) * 0.5f);
            mVariance = Utility.GetRandomVector2(-variance, variance, -variance, variance);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position + mVariance);
            Rotation += Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget))) * 0.1f;
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
