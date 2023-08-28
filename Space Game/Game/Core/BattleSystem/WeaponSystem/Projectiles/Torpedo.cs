using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem.Projectiles
{
    public class Torpedo : Projectile
    {
        public Torpedo(Vector2 position, SpaceShip targetObj, string texture, int shieldDamage, int hullDamage) 
            : base(position, targetObj, texture, 0.25f, shieldDamage, hullDamage, 2.5f) { }

        public override void Update(Vector2 startPosition, GameTime gameTime, InputState inputState)
        {
            RemoveFromSpatialHashing();
            var angleToTarget = Geometry.AngleBetweenVectors(Position, mTargetObj.Position + mVariance);
            Rotation += Geometry.DegToRad(Geometry.AngleDelta(Geometry.RadToDeg(Rotation), Geometry.RadToDeg(angleToTarget))) * 0.1f;
            Position += Geometry.CalculateDirectionVector(Rotation) * mVelocity * gameTime.ElapsedGameTime.Milliseconds;
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(startPosition, gameTime, inputState);
            AddToSpatialHashing();
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
