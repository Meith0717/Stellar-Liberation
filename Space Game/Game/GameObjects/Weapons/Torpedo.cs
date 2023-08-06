using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;


namespace CelestialOdyssey.Game.GameObjects.Weapons
{
    public enum ProjectileType
    {
        None
    }

    internal class Torpedo : GameObject
    {
        public float LiveTime = 0;
        private float mVelocity;
        private GameObject mTargetObj;

        public Torpedo(Vector2 position, GameObject targetObj, float rotation, ProjectileType type)
            : base(position, "projectile", 0.2f, 1)
        {
            mTargetObj = targetObj;
            Rotation = rotation;
            mVelocity = 0.6f;
            TextureColor = Color.IndianRed;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position);
            Rotation += Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget))) * 0.1f;
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            LiveTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
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
