// Enemy.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement.ShipSystems;
using StellarLiberation.Game.GameObjects.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.GameObjects.SpaceShipManagement.ShipSystems.WeaponSystem;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceShipManagement
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
