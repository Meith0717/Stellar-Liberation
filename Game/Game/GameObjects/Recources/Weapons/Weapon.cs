// Weapon.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.GameObjects.Recources.Weapons
{
    public class Weapon : Item
    {
        private Vector2 mOnShipPosition;
        private Vector2 mPosition;
        private float mRotation;
        private readonly Spaceship mSpaceship;
        private readonly string mTextureID;
        private readonly string mProjectileTextureID;
        private readonly Color mProjectileColor;
        private readonly bool mProjectileFollowTarget;
        public readonly float HullDamage;
        public readonly float ShieldDamage;

        public Weapon(Vector2 onShipPosition, Spaceship spaceShip, string objectTextureID, string itemTextureId, ItemID itemID, string projectileTextureID, Color projectileColor, float hullDamage, float shieldDamage, bool followTarget)
            : base(itemID, itemTextureId, false)
        {
            mOnShipPosition = onShipPosition;
            mSpaceship = spaceShip;
            mTextureID = objectTextureID;
            mProjectileTextureID = projectileTextureID;
            mProjectileColor = projectileColor;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            mProjectileFollowTarget = followTarget;
        }

        public void Fire(GameLayer gameLayer, Spaceship target)
        {
            //gameLayer.GameObjectsManager.Add(new WeaponProjectile(mPosition, mRotation, mSpaceship, target, HullDamage, ShieldDamage, mProjectileFollowTarget, mProjectileTextureID, mProjectileColor));
        }

        public void Update(float rotation)
        {
            var shipPosition = mSpaceship.Position;
            var shipRotation = mSpaceship.Rotation;
            mPosition = Transformations.Rotation(shipPosition, mOnShipPosition, shipRotation);
            mRotation = rotation;
        }

        public void Draw() => TextureManager.Instance.Draw(mTextureID, mPosition, 1f, mRotation, 11, Color.White);
    }
}
