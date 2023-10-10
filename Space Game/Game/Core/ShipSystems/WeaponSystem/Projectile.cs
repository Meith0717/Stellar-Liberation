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
        public double LiveTime { get; private set; }
        public SpaceShip Origine { get; private set; }

        private Vector2 mDirection;
        public readonly int ShieldDamage;
        public readonly int HullDamage;

        private bool mHasHit;
        public void Hit() => mHasHit = true;

        public Projectile(Vector2 weaponPosition, float rotation, Color color, int shieldDamage, int hullDamage, SpaceShip origine)
            : base(weaponPosition, ContentRegistry.projectile, 0.5f, 20)
        {
            mDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            TextureColor = color;
            LiveTime = 5000;
            Origine = origine;
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            LiveTime -= mHasHit ? double.PositiveInfinity : gameTime.ElapsedGameTime.Milliseconds;

            // Update Position
            RemoveFromSpatialHashing(scene);
            Position += mDirection * (float)(15f * gameTime.ElapsedGameTime.Milliseconds);
            AddToSpatialHashing(scene);

            base.Update(gameTime, inputState, sceneManagerLayer, scene);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
