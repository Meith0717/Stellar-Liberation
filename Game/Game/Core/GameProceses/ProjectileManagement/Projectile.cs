// Projectile.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.Core.GameProceses.ProjectileManagement
{
    public class Projectile : GameObject2D
    {
        public SpaceShip Origine { get; private set; }

        public readonly float ShieldDamage;
        public readonly float HullDamage;

        private readonly Turret mTurret;

        public Projectile(Turret turret, float rotation, Color color, float shieldDamage, float hullDamage, SpaceShip origine)
            : base(turret.Position, TextureRegistries.projectile, 1f, 15)
        {
            mTurret = turret;
            MovingDirection = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            TextureColor = color;
            Origine = origine;
            Velocity = 50;
            DisposeTime = 500;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }

        public override void HasCollide(Vector2 position, Scene scene)
        {
            Dispose = true;
            RemoveFromSpatialHashing(scene);
            Position = position;
            AddToSpatialHashing(scene);
            Velocity = 0;
        }
    }
}
