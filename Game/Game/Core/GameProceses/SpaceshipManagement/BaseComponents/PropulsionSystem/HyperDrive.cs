// HyperDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components.PropulsionSystem
{
    public class HyperDrive
    {
        private const int CoolDown = 5500;
        public bool IsActive { get; private set; }
        private PlanetsystemState TargetPlanetSystem;
        private float mActualChargingTime;
        private float mEngineCoolDownTime;
        private float mActualEngineCoolDownTime;

        public double ActualCharging => mActualEngineCoolDownTime / mEngineCoolDownTime;

        public void SetTarget(PlanetsystemState planetSystem)
        {
            if (mEngineCoolDownTime > mActualEngineCoolDownTime) return;
            IsActive = true;
            mActualChargingTime = 0;
            TargetPlanetSystem = planetSystem;
        }

        public void Update(GameTime gameTime, Spaceship operatingShip, PlanetsystemState planetsystemState)
        {
            mActualEngineCoolDownTime += gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (TargetPlanetSystem is null) return;
            HyperDriveEffect.Charge(operatingShip.Position, planetsystemState.ParticleEmitors);
            if (CoolDown >= mActualChargingTime) return;
            planetsystemState.RemoveGameObject(operatingShip);
            TargetPlanetSystem.AddGameObject(operatingShip);
            HyperDriveEffect.Stop(operatingShip.Position, TargetPlanetSystem.ParticleEmitors, 1);
            TargetPlanetSystem = null;
            IsActive = false;
        }
    }
}