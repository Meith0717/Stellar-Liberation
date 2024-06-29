// BattleshipFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System.Collections.Generic;
using System.Text.Json;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts
{
    public sealed class BattleshipFactory
    {
        private static BattleshipFactory mInstance;
        public static BattleshipFactory Instance { get { return mInstance ??= new(); } }

        private readonly Dictionary<string, Dictionary<string, JsonElement>> mBattleshipConfigs = new();
        private readonly Dictionary<string, Dictionary<string, JsonElement>> mWeaponConfigs = new();

        public void AddConfig(string id, Dictionary<string, JsonElement> config) => mBattleshipConfigs[id] = config;
        public void AddWeapons(string id, Dictionary<string, JsonElement> weapons) => mWeaponConfigs[id] = weapons;

        public Battleship GetBattleShip(string shipId, Vector2 position, Fraction fraction)
        {
            var shipConfig = mBattleshipConfigs[shipId];
            var engineTrailPosition = shipConfig["EngineTrailPosition"].GetVector();
            var shieldForce = shipConfig["ShieldForce"].GetInt32();
            var hullForce = shipConfig["HullForce"].GetInt32();
            var shieldReg = shipConfig["ShieldRegeneration"].GetInt32();
            var hullReg = shipConfig["HullRegeneration"].GetInt32();
            var impulseVelocity = shipConfig["ImpulseVelocity"].GetInt32();

            var weapons = new List<Weapon>();
            foreach (var weaponID in mWeaponConfigs[shipId].Keys)
                foreach (var onShipPosition in mWeaponConfigs[shipId][weaponID].EnumerateArray())
                    weapons.Add(WeaponFactory.Instance.GetWeapon(onShipPosition.GetVector(), weaponID));

            var battleShip = new Battleship(shipId, position, fraction, shipId, 1f, engineTrailPosition);
            battleShip.Populate(shieldForce, hullForce, shieldReg, hullReg, weapons, impulseVelocity);
            return battleShip;
        }
    }
}
