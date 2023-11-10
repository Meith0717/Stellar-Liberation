// Enemy.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.Core.SpaceShipManagement
{
    [Serializable]
    public abstract class Enemy : SpaceShip
    {
        protected Enemy(Vector2 position, string textureId, float textureScale, SensorArray sensorArray, SublightEngine sublightEngine, TurretBattery weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, sensorArray, sublightEngine, weaponSystem, defenseSystem, Factions.Allies)
        { }
    }
}
