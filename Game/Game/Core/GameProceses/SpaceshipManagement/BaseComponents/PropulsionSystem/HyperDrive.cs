﻿// HyperDrive.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components.PropulsionSystem
{
    public class HyperDrive
    {
        private const int CoolDown = 0;
        public bool IsActive { get; private set; }
        private PlanetSystem TargetPlanetSystem;
        private float mActualChargingTime;
        private float mEngineCoolDownTime;
        private float mActualEngineCoolDownTime;

        public HyperDrive() => mEngineCoolDownTime = mActualEngineCoolDownTime = CoolDown;

        public double ActualCharging => mActualEngineCoolDownTime / mEngineCoolDownTime;

        public void SetTarget(PlanetSystem planetSystem)
        {
            if (mEngineCoolDownTime > mActualEngineCoolDownTime) return;
            // SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.ChargeHyperdrive);
            IsActive = true;
            mActualChargingTime = 0;
            TargetPlanetSystem = planetSystem;
        }

        public void Update(GameTime gameTime, Spaceship operatingShip, GameLayer scene)
        {
            mActualEngineCoolDownTime += gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (TargetPlanetSystem is null) return;
            HyperDriveEffect.Charge(operatingShip.Position, scene.ParticleManager);
            if (CoolDown > mActualChargingTime) return;
            scene.GameState.SpaceShips.ChangePlanetSystem(scene.GameState.SpaceShips.LocateSpaceShip(operatingShip), TargetPlanetSystem, operatingShip);
            TargetPlanetSystem = null;
            IsActive = false;
        }
    }
}