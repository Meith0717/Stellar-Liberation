// HyperDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    [Serializable]
    public class HyperDrive
    {
        private const int CoolDown = 5000;

        [JsonProperty] private float mActualChargingTime;
        [JsonProperty] private PlanetsystemState TargetPlanetSystem;

        public HyperDrive(float velocityPerc) {; }

        public void SetTarget(PlanetsystemState planetSystem) => TargetPlanetSystem = planetSystem;

        public void Update(GameTime gameTime, Spaceship operatingShip, PlanetsystemState planetsystemState)
        {
            if (TargetPlanetSystem is null) return;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;
            HyperDriveEffect.Charge(operatingShip.Position, planetsystemState.ParticleEmitors);
            if (CoolDown >= mActualChargingTime) return;
            planetsystemState.RemoveGameObject(operatingShip);
            TargetPlanetSystem.AddGameObject(operatingShip);
            HyperDriveEffect.Stop(operatingShip.Position, TargetPlanetSystem.ParticleEmitors, 1);
            TargetPlanetSystem = null;
            mActualChargingTime = 0;
        }
    }
}