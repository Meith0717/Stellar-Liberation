// Weapon.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents.Weapons
{
    [Serializable]
    public class Weapon
    {
        [JsonProperty] public float HullDamage { get; private set; }
        [JsonProperty] public float ShieldDamage { get; private set; }
        [JsonProperty] public float Range { get; private set; }
        [JsonProperty] private Vector2 mOnShipPosition;
        [JsonProperty] private Vector2 mPosition;
        [JsonProperty] private float mRotation;
        [JsonProperty] private readonly string mTextureID;
        [JsonProperty] private readonly string mProjectileTextureID;
        [JsonProperty] private readonly Color mProjectileColor;
        [JsonProperty] private readonly bool mProjectileFollowTarget;
        [JsonProperty] private readonly double mCoolDown;
        [JsonProperty] private double mActualCoolDown;

        public Weapon(Vector2 onShipPosition, string objectTextureID, string projectileTextureID, Color projectileColor, bool followTarget, float hullDamage, float shieldDamage, float range, double coolDown)
        {
            mOnShipPosition = onShipPosition;
            mTextureID = objectTextureID;
            mProjectileTextureID = projectileTextureID;
            mProjectileColor = projectileColor;
            mProjectileFollowTarget = followTarget;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            Range = range;
            mCoolDown = coolDown;
        }

        public void Boost(float hullDamagePerc, float shieldDamagePerc, float rangePerc)
        {
            HullDamage *= hullDamagePerc;
            ShieldDamage *= shieldDamagePerc;
            Range *= rangePerc;
        }

        public void Fire(PlanetsystemState planetsystemState, Spacecraft spacecraft, Spacecraft target)
        {
            if (mActualCoolDown < mCoolDown) return;
            var projectile = new WeaponProjectile(mPosition, mProjectileTextureID);
            projectile.Populate(spacecraft, target, mRotation, HullDamage, ShieldDamage, Range, mProjectileFollowTarget, mProjectileColor);
            planetsystemState.AddGameObject(projectile);
            mActualCoolDown = 0;
        }

        public void Update(GameTime gameTime, Spacecraft spacecraft, float rotation)
        {
            mActualCoolDown += gameTime.ElapsedGameTime.TotalMilliseconds;
            var shipPosition = spacecraft.Position;
            var shipRotation = spacecraft.Rotation;
            mPosition = Transformations.Rotation(shipPosition, mOnShipPosition, shipRotation);
            mRotation = rotation;
        }

        public void Draw() 
            => TextureManager.Instance.Draw(mTextureID, mPosition, 5f, mRotation, 20, Color.White);
    }
}
