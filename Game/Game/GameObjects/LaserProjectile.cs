// LaserProjectile.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.GameObjects
{
    public class LaserProjectile : GameObject2D
    {
        public readonly Fractions Fraction;

        public readonly float ShieldDamage;
        public readonly float HullDamage;

        public LaserProjectile(Vector2 position, float rotation, Color color, float shieldDamage, float hullDamage, Fractions fraction)
            : base(position, GameSpriteRegistries.projectile, 1f, 15)
        {
            MovingDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            TextureColor = color;
            Fraction = fraction;
            Velocity = 15;
            DisposeTime = 2000;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
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
