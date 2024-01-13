// EnemyShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using StellarLiberation.Game.Core.Visuals;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys
{
    [Serializable]
    public abstract class EnemyShip : SpaceShip
    {
        [JsonIgnore] public bool IsSpotted { get; set; } = false;
        [JsonIgnore] private readonly RadarPing mRadarPing;

        protected EnemyShip(Vector2 position, string textureId, float textureScale, SensorSystem sensorArray, SublightDrive sublightEngine, TurretSystem weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, sensorArray, sublightEngine, weaponSystem, defenseSystem, Fractions.Enemys)
        {
            mRadarPing = new();
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            IsSpotted = false;
            mRadarPing.Update(Position, gameTime);
        }

        public override void Draw(Scene scene)
        {
            if (!IsSpotted)
            {
                scene.GameLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ShortRangeScanDistance, scene);
                mRadarPing.Draw(scene);
                return;
            }
            base.Draw(scene);
        }
    }
}
