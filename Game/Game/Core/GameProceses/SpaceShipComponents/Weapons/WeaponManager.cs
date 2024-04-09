// WeaponArray.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents.Weapons
{
    [Serializable]
    public class WeaponManager
    {
        [JsonProperty] private readonly List<Weapon> mWeapons;
        [JsonProperty] private bool mFire;

        public WeaponManager(List<Weapon> weapons) => mWeapons = weapons;

        public void Fire() => mFire = true;

        public void StopFire()=> mFire = false;

        public void Update(GameTime gameTime, Spacecraft spaceCraft, PlanetsystemState planetsystemState)
        {
            foreach (var weapon in mWeapons)
            {
                weapon.Update(gameTime, spaceCraft, spaceCraft.Rotation);
                if (!mFire) continue;
                weapon.Fire(planetsystemState, spaceCraft, null);
            }
        }

        public void Boost(float hullDamagePerc, float shieldDamagePerc, float rangePerc)
        {
            foreach (var weapon in mWeapons)
                weapon.Boost(hullDamagePerc, shieldDamagePerc, rangePerc);
        }

        public void Draw()
        {
            foreach (var weapon in mWeapons)
                weapon.Draw();
        }
    }
}
