// Weapon.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons
{
    [Serializable]
    public class Weapon
    {
        // Config Stuff
        [JsonProperty] public float HullDamage { get; private set; }
        [JsonProperty] public float ShieldDamage { get; private set; }
        [JsonProperty] public float Range { get; private set; }
        [JsonProperty] private readonly string mTextureID;
        [JsonProperty] private readonly string mProjectileTextureID;
        [JsonProperty] private readonly Color mColor;
        [JsonProperty] private readonly bool mFollowTarget;
        [JsonProperty] private readonly double mCoolDown;

        // Some Other stuff
        [JsonProperty] public Vector2 Position { get; private set; }
        [JsonProperty] private float mRotation;
        [JsonProperty] private Vector2 mOnShipPosition;
        [JsonProperty] private double mActualCoolDown;

        public Weapon(Vector2 onShipPosition, string objectTextureID, string projectileTextureID, Color projectileColor, bool followTarget, float hullDamage, float shieldDamage, float range, double coolDown)
        {
            mOnShipPosition = onShipPosition;
            mTextureID = objectTextureID;
            mProjectileTextureID = projectileTextureID;
            mColor = projectileColor;
            mFollowTarget = followTarget;
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
            if (Vector2.Distance(target.Position, Position) > Range) return;
            var projectile = new WeaponProjectile(Position, mProjectileTextureID);
            projectile.Populate(spacecraft, target, mRotation, HullDamage, ShieldDamage, Range, mFollowTarget, mColor);
            planetsystemState.AddGameObject(projectile);
            mActualCoolDown = 0;
        }

        public void Update(GameTime gameTime, Spacecraft spacecraft, float rotation)
        {
            mActualCoolDown += gameTime.ElapsedGameTime.TotalMilliseconds;
            var shipPosition = spacecraft.Position;
            var shipRotation = spacecraft.Rotation;
            Position = Transformations.Rotation(shipPosition, mOnShipPosition, shipRotation);
            mRotation = rotation;
        }

        public void Draw(float scale, Color color)
            => TextureManager.Instance.Draw(mTextureID, Position, scale * 0.75f, mRotation, 20, color);
    }
}
