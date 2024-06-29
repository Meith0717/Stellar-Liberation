// WeaponFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Extensions;
using System.Collections.Generic;
using System.Text.Json;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons
{
    public sealed class WeaponFactory
    {
        private static WeaponFactory mInstance;
        public static WeaponFactory Instance { get { return mInstance ??= new(); } }

        private readonly Dictionary<string, Dictionary<string, JsonElement>> mWeaponConfigs = new();

        public void AddConfig(string name, Dictionary<string, JsonElement> config) => mWeaponConfigs[name] = config;

        public Weapon GetWeapon(Vector2 onShipPosition, string iD)
        {
            mWeaponConfigs.TryGetValue(iD, out var weaponConfig);
            var hullDamage = (float)weaponConfig["HullDamage"].GetInt32();
            var shieldDamage = (float)weaponConfig["ShieldDamage"].GetInt32();
            var coolDown = (float)weaponConfig["CoolDown"].GetInt32();
            var range = (float)weaponConfig["Range"].GetInt32();
            var followTarget = weaponConfig["FollowTarget"].GetBoolean();
            var color = weaponConfig["Color"].GetColor();

            return new Weapon(onShipPosition, $"{iD}Obj", $"{iD}Proj", color, followTarget, hullDamage, shieldDamage, range, coolDown);
        }
    }
}
