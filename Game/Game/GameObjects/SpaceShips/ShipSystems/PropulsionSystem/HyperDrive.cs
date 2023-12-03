// HyperDrive.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;

namespace StellarLiberation.Game.GameObjects.SpaceShipManagement.ShipSystems.PropulsionSystem
{
    public class HyperDrive
    {
        public bool IsActive { get; private set; }
        public Vector2? TargetPosition { get; private set; }
        private float mMaxVelocity;
        private float mChargingTime;
        private float mActualChargingTime;
        private float mEngineCoolDownTime;
        private float mActualEngineCoolDownTime;
        private float mLastDistanceToTarget;

        public HyperDrive(float maxVelocity, float engineCoolDownTime)
        {
            mMaxVelocity = maxVelocity;
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
            TargetPosition = planetSystem.Star.Position;
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip)
        {
            mActualEngineCoolDownTime += gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            GetVelocity(spaceShip);
            if (TargetPosition is null) return;
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)TargetPosition) * 0.05f;
            ManageDistance(spaceShip, gameTime);
        }

        private void GetVelocity(SpaceShip spaceShip)
        {
            if (mChargingTime > mActualChargingTime)
            {
                if (TargetPosition is null) return;
                mLastDistanceToTarget = Vector2.Distance(spaceShip.Position, (Vector2)TargetPosition);
                return;
            }
            switch (TargetPosition)
            {
                case null:
                    if (!IsActive) return;
                    if (spaceShip.Velocity > 0) spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, 0, -mMaxVelocity / 5);
                    if (spaceShip.Velocity <= 0)
                    {
                        SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.CoolHyperdrive);
                        spaceShip.Velocity = 0;
                        mActualEngineCoolDownTime = 0;
                        IsActive = false;
                    }
                    break;
                case not null:
                    if (spaceShip.Velocity < mMaxVelocity) spaceShip.Velocity = MovementController.GetVelocity(spaceShip.Velocity, mMaxVelocity, mMaxVelocity / 5);
                    break;
            }
        }

        private float GetDistanceToTarget(SpaceShip spaceShip, GameTime gameTime)
        {
            return Vector2.Distance(spaceShip.Position, (Vector2)TargetPosition)
                - mMaxVelocity * gameTime.ElapsedGameTime.Milliseconds;
        }

        private void ManageDistance(SpaceShip spaceShip, GameTime gameTime)
        {
            var distanceToTarget = GetDistanceToTarget(spaceShip, gameTime);

            if (mLastDistanceToTarget >= distanceToTarget)
            {
                mLastDistanceToTarget = distanceToTarget;
                return;
            }
            TargetPosition = null;
        }
    }
}