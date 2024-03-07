// Weapon.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.WeaponElements
{
    public class Weapon
    {
        private readonly Spaceship mSpaceship;
        private Vector2 mOnShipPosition;
        private Vector2 mPosition;
        private float mRotation;

        public Weapon(Vector2 onShipPosition, Spaceship spaceShip)
        {
            mOnShipPosition = onShipPosition;
            mSpaceship = spaceShip;
        }

        public void Fire(GameObject2DManager objManager, Color particleColor, float shieldDamage, float hullDamage)
            => objManager.SpawnGameObject2D(new LaserProjectile(mPosition, mRotation, particleColor, shieldDamage, hullDamage, mSpaceship.Fraction));

        private void Update(float rotation)
        {
            var shipPosition = mSpaceship.Position;
            var shipRotation = mSpaceship.Rotation;
            mPosition = Transformations.Rotation(shipPosition, mOnShipPosition, shipRotation);
            mRotation = rotation;
        }

        public void Draw()
        {
            TextureManager.Instance.Draw(GameSpriteRegistries.turette, mPosition, 1f, mRotation, 11, Color.White);
        }
    }
}
