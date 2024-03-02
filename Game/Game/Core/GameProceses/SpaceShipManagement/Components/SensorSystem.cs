// SensorSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components
{
    public class SensorSystem
    {
        public const int ShortRangeScanDistance = 15000;
        private const int MaxCoolDown = 500;

        public List<GameObject2D> LongRangeScan => mLongRangeScan;
        public List<GameObject2D> ShortRangeScan => mShortRangeScan;
        public List<SpaceShip> OpponentsInRannge => mOpponentsInRannge;
        public List<SpaceShip> AlliesInRannge => mAlliesInRannge;
        public List<SpaceShip> Opponents => mOpponents;
        public List<SpaceShip> Allies => mAllies;

        private List<GameObject2D> mLongRangeScan = new();
        private List<GameObject2D> mShortRangeScan = new();
        private List<SpaceShip> mOpponentsInRannge = new();
        private List<SpaceShip> mAlliesInRannge = new();
        private List<SpaceShip> mOpponents = new();
        private List<SpaceShip> mAllies = new();

        private int mCoolDown;

        public SensorSystem() => mCoolDown = ExtendetRandom.Random.Next(MaxCoolDown);

        public void Scan(GameTime gameTime, PlanetSystem planetSystem, Vector2 spaceShipPosition, Fractions fraction, GameLayer scene)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = MaxCoolDown;

            mOpponents = planetSystem.GameObjects.OfType<SpaceShip>().Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAllies = planetSystem.GameObjects.OfType<SpaceShip>().Where((spaceShip) => spaceShip.Fraction == fraction).ToList();

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

            mOpponentsInRannge = mShortRangeScan.OfType<SpaceShip>().Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAlliesInRannge = mShortRangeScan.OfType<SpaceShip>().Where((spaceShip) => spaceShip.Fraction == fraction).ToList();
        }

        public bool TryGetAimingShip(Vector2 spaceShipPosition, out SpaceShip spaceShip)
        {
            PriorityQueue<SpaceShip, double> q = new();
            foreach (var spaceShip1 in mOpponentsInRannge)
                q.Enqueue(spaceShip1, -GetAimingScore(spaceShipPosition, spaceShip1));
            return q.TryDequeue(out spaceShip, out var _);
        }

        private double GetAimingScore(Vector2 position, SpaceShip spaceShip)
        {
            var spaceShipShielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.5 + spaceShip.DefenseSystem.HullPercentage * 0.5;
            var distanceScore = Vector2.Distance(position, spaceShip.Position) / ShortRangeScanDistance;
            return (1 - spaceShipShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
        }
    }
}
