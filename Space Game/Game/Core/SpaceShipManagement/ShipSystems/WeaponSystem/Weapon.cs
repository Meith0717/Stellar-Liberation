// Weapon.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    internal class Weapon : GameObject
    {
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

        public Projectile Fire(SpaceShip origin) => new(Position, Rotation, mWeaponColor, mShieldDamage, mHullDamage, origin);

        public void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene, float shipRotation, Vector2 shipPosition, Vector2? target)
        {
            Position = Transformations.Rotation(shipPosition, mRelativePosition, shipRotation);
            Rotation += target switch
            {
                null => MovementController.GetRotationUpdate(Rotation, shipRotation, 1),
                not null => MovementController.GetRotationUpdate(Rotation, Position, (Vector2)target, 0.5f),
            };
            base.Update(gameTime, inputState, gameLayer, scene);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}