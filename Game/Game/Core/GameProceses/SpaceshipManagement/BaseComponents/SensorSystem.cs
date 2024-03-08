// SensorSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components
{
    public class SensorSystem
    {
        public const int ShortRangeScanDistance = 15000;
        private const int MaxCoolDown = 500;

        public List<GameObject2D> LongRangeScan => mLongRangeScan;
        public List<GameObject2D> ShortRangeScan => mShortRangeScan;
        public List<Spaceship> OpponentsInRannge => mOpponentsInRannge;
        public List<Spaceship> AlliesInRannge => mAlliesInRannge;
        public List<Spaceship> Opponents => mOpponents;
        public List<Spaceship> Allies => mAllies;

        private List<GameObject2D> mLongRangeScan = new();
        private List<GameObject2D> mShortRangeScan = new();
        private List<Spaceship> mOpponentsInRannge = new();
        private List<Spaceship> mAlliesInRannge = new();
        private List<Spaceship> mOpponents = new();
        private List<Spaceship> mAllies = new();

        private int mCoolDown;

        public SensorSystem() => mCoolDown = ExtendetRandom.Random.Next(MaxCoolDown);

        public void Scan(GameTime gameTime, Vector2 spaceShipPosition, Fractions fraction, GameLayer scene)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = MaxCoolDown;

            mOpponents = scene.GameObjects.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAllies = scene.GameObjects.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction == fraction).ToList();

            mOpponents.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(spaceShipPosition, obj1.Position);
                var distance2 = Vector2.DistanceSquared(spaceShipPosition, obj2.Position);
                return distance1.CompareTo(distance2);
            });
            mAllies.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(spaceShipPosition, obj1.Position);
                var distance2 = Vector2.DistanceSquared(spaceShipPosition, obj2.Position);
                return distance1.CompareTo(distance2);
            });

            mShortRangeScan.Clear();
            scene.SpatialHashing.GetObjectsInRadius(spaceShipPosition, ShortRangeScanDistance, ref mShortRangeScan);

            mOpponentsInRannge = mShortRangeScan.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAlliesInRannge = mShortRangeScan.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction == fraction).ToList();
        }

        public bool TryGetAimingShip(Vector2 spaceShipPosition, out Spaceship spaceShip)
        {
            PriorityQueue<Spaceship, double> q = new();
            foreach (var spaceShip1 in mOpponentsInRannge)
                q.Enqueue(spaceShip1, -GetAimingScore(spaceShipPosition, spaceShip1));
            return q.TryDequeue(out spaceShip, out var _);
        }

        private double GetAimingScore(Vector2 position, Spaceship spaceShip)
        {
            var spaceShipShielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.5 + spaceShip.DefenseSystem.HullPercentage * 0.5;
            var distanceScore = Vector2.Distance(position, spaceShip.Position) / ShortRangeScanDistance;
            return (1 - spaceShipShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
        }
    }
}
