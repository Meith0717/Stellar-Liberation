using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
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

        public double ActualCharging {  
            get { 
                return mActualEngineCoolDownTime/mEngineCoolDownTime;
            } 
        }

        public void SetTarget(PlanetSystem planetSystem, Player player, MapLayer mapLayer) 
        {
            if (mEngineCoolDownTime > mActualEngineCoolDownTime) return;
            SoundManager.Instance.PlaySound(ContentRegistry.ChargeHyperdrive, 1);
            mapLayer.CloseMap();
            player.Velocity = 50;
            IsActive = true;
            mActualChargingTime = 0;
            TargetPosition = planetSystem.Star.Position;
            mLastDistanceToTarget = Vector2.Distance(player.Position, (Vector2)TargetPosition);
        }

        public void Update(GameTime gameTime, Player player) 
        {
            mActualEngineCoolDownTime +=  gameTime.ElapsedGameTime.Milliseconds;
            mActualChargingTime += gameTime.ElapsedGameTime.Milliseconds;

            GetVelocity(player);
            if (TargetPosition is null) return;
            player.Rotation += MovementController.GetRotationUpdate(player.Rotation, player.Position, (Vector2)TargetPosition, 0.05f);
            ManageDistance(player, gameTime);
        }

        private void GetVelocity(Player player)
        {
            if (mChargingTime > mActualChargingTime) return;
            switch (TargetPosition)
            {
                case null:
                    if (!IsActive) return;
                    if (player.Velocity > 0) player.Velocity = MovementController.GetVelocity(player.Velocity, -mMaxVelocity / 5);
                    if (player.Velocity <= 0)
                    {
                        SoundManager.Instance.PlaySound(ContentRegistry.CoolHyperdrive, 1);
                        player.Velocity = 0;
                        mActualEngineCoolDownTime = 0;
                        IsActive = false;
                    }
                    break;
                case not null:
                    if (player.Velocity < mMaxVelocity) player.Velocity = MovementController.GetVelocity(player.Velocity, mMaxVelocity / 5);
                    break;
            }
        }

        private float GetDistanceToTarget(Player player, GameTime gameTime)
        {
            return Vector2.Distance(player.Position, (Vector2)TargetPosition)
                - (mMaxVelocity * gameTime.ElapsedGameTime.Milliseconds);
        }

        private void ManageDistance(Player player, GameTime gameTime)
        {
            var distanceToTarget = GetDistanceToTarget(player, gameTime);

            if (mLastDistanceToTarget >= distanceToTarget)
            {
                mLastDistanceToTarget = distanceToTarget;
                return;
            }
            TargetPosition = null;
        }
    }
}