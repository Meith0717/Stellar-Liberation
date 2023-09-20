using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class Projectile : GameObject
    {
        private Vector2 mDirection;
        private float mVelocity;
        public double LiveTime { get; private set; }
        public readonly int ShieldDamage;
        public readonly int HullDamage;
        public readonly SpaceShip Origin;
        public bool HasHit;

        public Projectile(SpaceShip origin, Vector2 startPosition, float rotation, int shieldDamage, int hullDamage, float velocity)
            : base(startPosition, ContentRegistry.projectile, 10, 20)
        {
            Origin = origin;
            mDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            mVelocity = velocity;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            LiveTime = 5000;
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            LiveTime -= HasHit ? double.PositiveInfinity : gameTime.ElapsedGameTime.Milliseconds;
            RemoveFromSpatialHashing(sceneLayer);
            Position += mDirection * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            AddToSpatialHashing(sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
