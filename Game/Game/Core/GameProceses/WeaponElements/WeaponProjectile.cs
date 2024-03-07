// WeaponProjectiles.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.WeaponElements
{
    internal class WeaponProjectile : GameObject2D
    {
        public readonly SpaceShip Shooter;
        public readonly SpaceShip Target;
        public readonly bool mFollowTarget;
        public readonly float ShieldDamage;
        public readonly float HullDamage;

        public WeaponProjectile(Vector2 startPosition, float shootRotation, SpaceShip shooter, SpaceShip target, float shieldDamage, float hullDamage, bool followTarget, string textureId, Color color)
            : base(startPosition, textureId, 1f, 15)
        {
            Shooter = shooter;
            Rotation = shootRotation;
            Target = target;
            ShieldDamage = shieldDamage;
            HullDamage = hullDamage;
            mFollowTarget = followTarget;
            TextureColor = color;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            if (mFollowTarget) Rotation = Geometry.AngleBetweenVectors(Position, Target.Position);
            GameObject2DMover.Move(gameTime, this, scene.SpatialHashing);
            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }

        public override void HasCollide(Vector2 position, GameLayer scene)
        {
            IsDisposed = true;
            Position = position;
            Velocity = 0;
        }
    }
}
