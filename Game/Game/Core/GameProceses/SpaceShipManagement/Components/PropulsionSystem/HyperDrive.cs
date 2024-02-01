// HyperDrive.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem
{
    public class HyperDrive
    {
        private const int CoolDown = 500;
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
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.ChargeHyperdrive);
            IsActive = true;
            mActualChargingTime = 0;
            TargetPlanetSystem = planetSystem;
        }

        public void Update(GameTime gameTime, SpaceShip operatingShip, GameLayer scene)
        {
            mActualEngineCoolDownTime += gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (TargetPlanetSystem is null) return;
            HyperDriveEffect.Charge(operatingShip.Position, scene.ParticleManager);
            if (CoolDown > mActualChargingTime) return;
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.CoolHyperdrive);
            scene.GameState.ChangePlanetSystem(TargetPlanetSystem);
            TargetPlanetSystem = null;
            IsActive = false;
        }
    }
}