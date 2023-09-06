using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem
{
    public class HyperDrive
    {
        public bool IsActive { get; private set; }
        public Vector2? TargetPosition { get; private set; }
        private float mMaxVelocity;
        private float mChargintTime;
        private float mActualChargingTime;
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
            TargetPosition = planetSystem.GetEntryPosition(player);
            mActualChargingTime = 0;
            mLastDistanceToTarget = Vector2.Distance(player.Position, (Vector2)TargetPosition);
        }

        public void Update(GameTime gameTime, Player player) 
        {
            mActualChargingTime = (mChargintTime > mActualChargingTime) ? mActualChargingTime + gameTime.ElapsedGameTime.Milliseconds : mChargintTime;
            GetVelocity(player);
            if (TargetPosition is null) return;
            var rotationToTarget = Geometry.AngleBetweenVectors(player.Position, (Vector2)TargetPosition);
            player.Rotation += MovementController.GetRotationUpdate(player.Rotation, rotationToTarget, 0.05f);
            ManageDistance(player, gameTime);
        }

        private void GetVelocity(Player player)
        {
            if (mChargintTime > mActualChargingTime) return; ;

            switch (TargetPosition)
            {
                case null:
                    if (!IsActive) return;
                    if (player.Velocity > 0) player.Velocity = MovementController.GetVelocity(player.Velocity, -mMaxVelocity / 10);
                    if (player.Velocity <= 0)
                    {
                        player.Velocity = 0;
                        IsActive = false;
                    }
                    break;
                case not null:
                    if (player.Velocity < mMaxVelocity) player.Velocity = MovementController.GetVelocity(player.Velocity, mMaxVelocity / 10);
                    IsActive = true;
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