// SensorSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    public class Sensors
    {
        private const int MaxCoolDown = 500;
        private int mCoolDown;

        public List<GameObject2D> LongRangeScan => mLongRangeScan;
        public List<Spaceship> Opponents => mOpponents;
        public List<Spaceship> Allies => mAllies;

        private List<GameObject2D> mLongRangeScan = new();
        private List<Spaceship> mOpponents = new();
        private List<Spaceship> mAllies = new();


        public Sensors() => mCoolDown = ExtendetRandom.Random.Next(MaxCoolDown);

        public void Scan(GameTime gameTime, Spaceship spaceship, Fractions fraction, PlanetsystemState planetsystemState)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = MaxCoolDown;

            mOpponents = planetsystemState.GameObjects.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAllies = planetsystemState.GameObjects.OfType<Spaceship>().Where((spaceShip) => spaceShip.Fraction == fraction).ToList();

            mOpponents.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(spaceship.Position, obj1.Position);
                var distance2 = Vector2.DistanceSquared(spaceship.Position, obj2.Position);
                return distance1.CompareTo(distance2);
            });
            mAllies.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(spaceship.Position, obj1.Position);
                var distance2 = Vector2.DistanceSquared(spaceship.Position, obj2.Position);
                return distance1.CompareTo(distance2);
            });
        }

        public bool TryGetAimingShip(Vector2 spaceShipPosition, out Spaceship spaceShip, int weaponRange)
        {
            PriorityQueue<Spaceship, double> q = new();
            foreach (var spaceShip1 in mOpponents)
                q.Enqueue(spaceShip1, -GetAimingScore(spaceShipPosition, spaceShip1, weaponRange));
            return q.TryDequeue(out spaceShip, out var _);
        }

        private static double GetAimingScore(Vector2 position, Spaceship spaceShip, int weaponRange)
        {
            var spaceShipShielHhullScore = spaceShip.Defense.ShieldPercentage * 0.5 + spaceShip.Defense.HullPercentage * 0.5;
            var distanceScore = Vector2.Distance(position, spaceShip.Position) / weaponRange;
            return (1 - spaceShipShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
        }
    }
}
