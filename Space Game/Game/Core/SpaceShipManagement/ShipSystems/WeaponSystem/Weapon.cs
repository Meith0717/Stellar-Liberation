// Weapon.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    internal class Weapon : GameObject
    {
        private Vector2? mTarget;
        private Vector2 mRelativePosition;
        private Color mWeaponColor;
        private int mShieldDamage;
        private int mHullDamage;

        public Weapon(Vector2 relativePosition, float textureScale, int textureDepth, Color color, int shieldDamage, int hullDamage)
        : base(Vector2.Zero, ContentRegistry.turette, textureScale, textureDepth)
        {
            mWeaponColor = color;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
            mRelativePosition = relativePosition;
        }

        public void SetTarget(Vector2 target) => mTarget = target;
        public void ForgetTarget() => mTarget = null;

        public bool Fire(SpaceShip origin, out Projectile projectile)
        {
            projectile = null;
            if (mTarget is not null)
            {
                Vector2 target = (Vector2)mTarget;
                var angleBetweenTarget = Geometry.AngleBetweenVectors(Position, target);
                var angleToTarget = MathF.Abs(Geometry.AngleRadDelta(Rotation, angleBetweenTarget));
                if (angleToTarget > 5) return false;
            }
            projectile = new Projectile(Position, Rotation, mWeaponColor, mShieldDamage, mHullDamage, origin);
            return true;
        }

        public void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene, float shipRotation, Vector2 shipPosition)
        {
            Position = Transformations.Rotation(shipPosition, mRelativePosition, shipRotation);
            Rotation += mTarget switch
            {
                null => MovementController.GetRotationUpdate(Rotation, shipRotation, 1),
                not null => MovementController.GetRotationUpdate(Rotation, Position, (Vector2)mTarget, 0.5f),
            };
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}