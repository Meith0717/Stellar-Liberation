// EnemyShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys
{
    [Serializable]
    public abstract class EnemyShip : SpaceShip
    {

        protected EnemyShip(Vector2 position, string textureId, float textureScale, SensorSystem sensorArray, SublightDrive sublightEngine, TurretSystem weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, sensorArray, sublightEngine, weaponSystem, defenseSystem, Fractions.Enemys)
        { }
    }
}
