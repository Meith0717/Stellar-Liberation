// WeaponManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons
{
    [Serializable]
    public class WeaponManager
    {
        [JsonProperty] public readonly List<Weapon> Weapons;
        [JsonProperty] private Spacecraft mTarget;

        public WeaponManager(List<Weapon> weapons) => Weapons = weapons;

        public void AimTarget(Spacecraft target) => mTarget = target;

        public void Update(GameTime gameTime, Spacecraft spaceCraft, PlanetsystemState planetsystemState)
        {
            foreach (var weapon in Weapons)
            {
                if (mTarget is null)
                    weapon.Update(gameTime, spaceCraft, spaceCraft.Rotation);
                else
                {
                    weapon.Update(gameTime, spaceCraft, Geometry.AngleBetweenVectors(weapon.Position, mTarget.Position));
                    weapon.Fire(planetsystemState, spaceCraft, mTarget);
                }
            }
            if (mTarget is null) return;
            mTarget = mTarget.IsDisposed ? null : mTarget;
        }

        public void Boost(float hullDamagePerc, float shieldDamagePerc, float rangePerc)
        {
            foreach (var weapon in Weapons)
                weapon.Boost(hullDamagePerc, shieldDamagePerc, rangePerc);
        }

        public void Draw(float scale, Color color)
        {
            foreach (var weapon in Weapons)
                weapon.Draw(scale, color);
        }
    }
}
