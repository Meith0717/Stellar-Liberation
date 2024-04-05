// HyperDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    [Serializable]
    public class HyperDrive
    {
        private const int CoolDown = 1000;

        [JsonProperty] private float mActualChargingTime;
        [JsonProperty] private PlanetsystemState TargetPlanetSystem;

        public HyperDrive(float velocityPerc) {; }

        public void SetTarget(PlanetsystemState planetSystem) => TargetPlanetSystem = planetSystem;

        public void Move(GameTime gameTime, GameObject gameObject, PlanetsystemState planetsystemState)
        {
            if (TargetPlanetSystem is null) return;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;
            HyperDriveEffect.Charge(gameObject.Position, planetsystemState.ParticleEmitors);
            if (CoolDown >= mActualChargingTime) return;
            planetsystemState.RemoveGameObject(gameObject);
            TargetPlanetSystem.AddGameObject(gameObject);
            HyperDriveEffect.Stop(gameObject.Position, TargetPlanetSystem.ParticleEmitors, 1);
            TargetPlanetSystem = null;
            mActualChargingTime = 0;
        }
    }
}