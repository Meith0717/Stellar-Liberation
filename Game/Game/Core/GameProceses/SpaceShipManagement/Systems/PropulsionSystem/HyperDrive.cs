// HyperDrive.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem
{
    public class HyperDrive
    {
        public bool IsActive { get; private set; }
        private PlanetSystem TargetPlanetSystem;
        private float mChargingTime;
        private float mActualChargingTime;
        private float mEngineCoolDownTime;
        private float mActualEngineCoolDownTime;

        public HyperDrive(float engineCoolDownTime)
        {
            mEngineCoolDownTime = mActualEngineCoolDownTime = engineCoolDownTime;
            mChargingTime = 6000;
        }

        public double ActualCharging => mActualEngineCoolDownTime / mEngineCoolDownTime;

        public void SetTarget(PlanetSystem planetSystem)
        {
            if (mEngineCoolDownTime > mActualEngineCoolDownTime) return;
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.ChargeHyperdrive);
            IsActive = true;
            mActualChargingTime = 0;
            TargetPlanetSystem = planetSystem;
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            mActualEngineCoolDownTime += gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (mChargingTime > mActualChargingTime) return;
            if (TargetPlanetSystem is null) return;
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.CoolHyperdrive);
            scene.GameLayer.ChangePlanetSystem(TargetPlanetSystem);
            TargetPlanetSystem = null;
            IsActive = false;
        }

    }
}