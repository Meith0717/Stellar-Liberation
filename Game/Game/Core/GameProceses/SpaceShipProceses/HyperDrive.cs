// HyperDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class HyperDrive
    {
        private const int CoolDown = 10000;

        [JsonProperty] public float MaxVelocity { get; private set; }
        [JsonProperty] private float mActualChargingTime;
        [JsonProperty] private PlanetsystemState TargetPlanetSystem;
        [JsonIgnore] public bool IsActive { get; private set; }

        public HyperDrive(float velocity) { MaxVelocity = velocity; }

        public void Boos(float velocityPerc) => MaxVelocity *= velocityPerc;

        public void SetTarget(PlanetsystemState planetSystem) => TargetPlanetSystem = planetSystem;

        public void Update(GameTime gameTime, Flagship flagship, GameState gameState, PlanetsystemState planetsystemState)
        {
            IsActive = false;
            if (TargetPlanetSystem is null) return;
            IsActive = true;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;
            HyperDriveEffect.Charge(flagship.Position, planetsystemState.ParticleEmitors);
            if (CoolDown >= mActualChargingTime) return;
            flagship.IsDisposed = true;
            gameState.MapState.JumpInHyperSpace(flagship, planetsystemState, TargetPlanetSystem);
            TargetPlanetSystem = null;
            mActualChargingTime = 0;
        }
    }
}