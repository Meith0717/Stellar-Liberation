// Projectile.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class Projectile : GameObject2D
    {
        public SpaceShip Origine { get; private set; }

        public readonly int ShieldDamage;
        public readonly int HullDamage;

        private bool mHit;

        public Projectile(Vector2 startPosition, float rotation, Color color, int shieldDamage, int hullDamage, SpaceShip origine)
            : base(startPosition, TextureRegistries.projectile, 1f, 15)
        {
            MovingDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            TextureColor = color;
            Origine = origine;
            Velocity = 20;
            DisposeTime = 5000;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            if (!mHit) TextureManager.Instance.DrawGameObject(this);
        }

        public override void HasCollide()
        {
            Dispose = true;
            Velocity = 0;
            mHit = true;
        }
    }
}
