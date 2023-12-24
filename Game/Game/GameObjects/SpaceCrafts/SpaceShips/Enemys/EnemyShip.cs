// EnemyShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals;
using System;
using System.ComponentModel.DataAnnotations;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys
{
    [Serializable]
    public abstract class EnemyShip : SpaceShip
    {
        [JsonIgnore] public bool IsSpotted { get; set; } = false;
        [JsonIgnore] private readonly RadarPing mRadarPing;

        private bool debug = false;

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
            void act() => debug = true;
            GameObject2DInteractionManager.Manage(inputState, this, scene, act, act, act);
            if (debug) System.Diagnostics.Debugger.Break();
        }

        public override void Draw(Scene scene)
        {
            if (debug) System.Diagnostics.Debugger.Break();
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
