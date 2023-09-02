using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem.Projectiles
{
    public class Torpedo : Projectile
    {
        public Torpedo(Vector2 position, SpaceShip targetObj, string texture, int shieldDamage, int hullDamage) 
            : base(position, targetObj, texture, 5f, shieldDamage, hullDamage, 50f) { }

        public override void Update(Vector2 startPosition, GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            RemoveFromSpatialHashing(sceneLayer);
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position + mVariance);
            Rotation += Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget))) * 0.1f;
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(startPosition, gameTime, inputState, sceneLayer);
            AddToSpatialHashing(sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
