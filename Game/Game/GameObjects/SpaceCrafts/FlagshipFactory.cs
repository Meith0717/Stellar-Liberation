// FlagshipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    public sealed class FlagshipFactory
    {
        private static FlagshipFactory mInstance;
        public static FlagshipFactory Instance { get { return mInstance ??= new(); } }

        private readonly Dictionary<string, Dictionary<string, JsonElement>> mFlagshipConfigs = new();
        private readonly Dictionary<string, Dictionary<string, JsonElement>> mWeaponConfigs = new();

        public List<string> IDs => mWeaponConfigs.Keys.ToList();


        public void AddConfig(string shipId, Dictionary<string, JsonElement> config) => mFlagshipConfigs[shipId] = config;
        public void AddWeapons(string shipId, Dictionary<string, JsonElement> weapons) => mWeaponConfigs[shipId] = weapons;

        public Flagship GetFlagship(string shipId, Vector2 position, Fraction fraction)
        {
            var shipConfig = mFlagshipConfigs[shipId];
            var engineTrailPosition = shipConfig["EngineTrailPosition"].GetVector();
            var shieldForce = shipConfig["ShieldForce"].GetInt32();
            var hullForce = shipConfig["HullForce"].GetInt32();
            var shieldReg = shipConfig["ShieldRegeneration"].GetInt32();
            var hullReg = shipConfig["HullRegeneration"].GetInt32();
            var impulseVelocity = shipConfig["ImpulseVelocity"].GetInt32();
            var hyperVelocity = shipConfig["hHyperVelocity"].GetInt32();
            var hangarCapacity = shipConfig["HangarCapacity"].GetInt32();

            var weapons = new List<Weapon>();
            foreach (var weaponID in mWeaponConfigs[shipId].Keys)
                foreach (var onShipPosition in mWeaponConfigs[shipId][weaponID].EnumerateArray())
                    weapons.Add(WeaponFactory.Instance.GetWeapon(onShipPosition.GetVector(), weaponID));

            var flagship = new Flagship(shipId, position, fraction, engineTrailPosition);
            flagship.Populate(shieldForce, hullForce, shieldReg, hullReg, weapons, impulseVelocity, hyperVelocity, hangarCapacity);
            return flagship;
        }
    }
}