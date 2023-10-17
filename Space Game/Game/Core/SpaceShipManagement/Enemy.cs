// Enemy.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement
{
    [Serializable]
    public abstract class Enemy : SpaceShip
    {

        protected Enemy(Vector2 position, string textureId, float textureScale, SensorArray sensorArray, SublightEngine sublightEngine, HyperDrive hyperDrive, WeaponSystem weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, sensorArray, sublightEngine, hyperDrive, weaponSystem, defenseSystem)
        { }
    }
}
