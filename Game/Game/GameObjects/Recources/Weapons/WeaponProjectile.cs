// WeaponProjectile.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.GameObjects.Recources.Weapons
{
    public class WeaponProjectile : GameObject2D
    {
        public readonly Spaceship Shooter;
        public readonly Spaceship Target;
        public readonly bool mFollowTarget;
        public readonly float ShieldDamage;
        public readonly float HullDamage;

        public WeaponProjectile(Vector2 startPosition, float shootRotation, Spaceship shooter, Spaceship target, float shieldDamage, float hullDamage, bool followTarget, string textureId, Color color)
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

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            if (mFollowTarget && Target is not null) Rotation = Geometry.AngleBetweenVectors(Position, Target.Position);
            GameObject2DMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            base.Update(gameTime, gameState, planetsystemState);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
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
