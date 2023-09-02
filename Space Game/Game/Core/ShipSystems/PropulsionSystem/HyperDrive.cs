using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    public class HyperDrive
    {
        public bool IsActive { get; private set; }
        private float mMaxVelocity;
        private float mChargintTime;
        private float mActualChargingTime;
        private Vector2? mTargetPosition;
        private float mLastDistanceToTarget;

        public HyperDrive(float maxVelocity, float chargintTime)
        {

            mMaxVelocity = maxVelocity;
            mChargintTime = chargintTime;
        }

        public double ActualCharging {  
            get { 
                return mActualChargingTime/mChargintTime;
            } 
        }

        public void SetTarget(PlanetSystem planetSystem, Player player) 
        {
            mTargetPosition = planetSystem.GetEntryPosition(player);
            mActualChargingTime = 0;
            mLastDistanceToTarget = Vector2.Distance(player.Position, (Vector2)mTargetPosition);
        }

        public void Update(GameTime gameTime, Player player) 
        {
            mActualChargingTime = (mChargintTime > mActualChargingTime) ? mActualChargingTime + gameTime.ElapsedGameTime.Milliseconds : mChargintTime;
            GetVelocity(player);
            if (mTargetPosition is null) return;
            var rotationToTarget = Geometry.AngleBetweenVectors(player.Position, (Vector2)mTargetPosition);
            player.Rotation += MovementController.GetRotationUpdate(player.Rotation, rotationToTarget, 0.05f);
            ManageDistance(player);
        }

        private void GetVelocity(Player player)
        {
            if (mChargintTime > mActualChargingTime) return; ;

            switch (mTargetPosition)
            {
                case null:
                    if (!IsActive) return;
                    if (player.Velocity > 0) player.Velocity = MovementController.GetVelocity(player.Velocity, -mMaxVelocity / 50);
                    if (player.Velocity <= 0)
                    {
                        IsActive = false;
                        player.Velocity = 0;
                    }
                    break;
                case not null:
                    if (player.Velocity < mMaxVelocity) player.Velocity = MovementController.GetVelocity(player.Velocity, mMaxVelocity / 50);
                    IsActive = true;
                    break;
            }
        }

        private void ManageDistance(Player player)
        {
            var distanceToTarget = Vector2.Distance(player.Position, (Vector2)mTargetPosition);
            if (mLastDistanceToTarget >= distanceToTarget)
            {
                mLastDistanceToTarget = distanceToTarget;
                return;
            }
            mTargetPosition = null;
        }
    }
}