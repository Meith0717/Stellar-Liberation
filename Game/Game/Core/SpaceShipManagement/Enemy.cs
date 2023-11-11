// Enemy.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using Microsoft.Xna.Framework;
using System;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;

namespace StellarLiberation.Game.Core.SpaceShipManagement
{
    [Serializable]
    public abstract class Enemy : SpaceShip
    {
        protected Enemy(Vector2 position, string textureId, float textureScale, SensorArray sensorArray, SublightEngine sublightEngine, TurretBattery weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, sensorArray, sublightEngine, weaponSystem, defenseSystem, Factions.Allies)
        { }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            SublightEngine.UpdateVelocity(this);
        }
    }
}
