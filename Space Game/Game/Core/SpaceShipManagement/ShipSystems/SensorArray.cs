// SensorArray.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems
{
    public class SensorArray
    {
        // Scan results
        public List<GameObject> AstronomicalObjects { get; private set; } = new();
        public List<SpaceShip> SortedSpaceShips { get; private set; } = new();
        public SpaceShip AimingShip { get; private set; }
        public float DistanceToAimingShip { get; private set; }

        // Scan atributes
        public int ShortRangeScanDistance { get; private set; }

        // Cooldown stuff
        private float mActualCounter;
        private float mMaxCounter = 100;

        public SensorArray(int shortRangeScanDistance)
        {            
            ShortRangeScanDistance = shortRangeScanDistance;
            mActualCounter = ExtendetRandom.Random.Next((int)mMaxCounter);
        }

        public void Update(GameTime gameTime, Vector2 position, PlanetSystem planetSystem, Scene scene, Factions opponent)
        {
            mActualCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (mActualCounter < mMaxCounter) return;
            mActualCounter = 0;

            AstronomicalObjects.Clear();
            AstronomicalObjects.Add(planetSystem.Star);
            AstronomicalObjects.AddRange(planetSystem.Planets);
            SortedSpaceShips = scene.GetObjectsInRadius<SpaceShip>(position, ShortRangeScanDistance);

            var opponents = new List<SpaceShip>(); 

            switch (opponent)
            {
                case Factions.Enemys:
                    var enemis = (IEnumerable<SpaceShip>)SortedSpaceShips.OfType<Enemy>();
                    opponents = enemis.ToList();
                    break;
                case Factions.Allies:
                    var allies = (IEnumerable<SpaceShip>)SortedSpaceShips.OfType<Player>();
                    opponents = allies.ToList();
                    break;
            }

             AimingShip =  GetAimingShip(position, opponents);

            DistanceToAimingShip = AimingShip is null ? float.PositiveInfinity : Vector2.Distance(position, AimingShip.Position);
        }

        private SpaceShip GetAimingShip(Vector2 position, List<SpaceShip> opponents)
        {
            PriorityQueue<SpaceShip, double> q = new();
            foreach (var opponent in opponents) q.Enqueue(opponent, -GetAimingScore(position, opponent));
            q.TryDequeue(out var spaceShip, out var _);
            return spaceShip;
        }

        private double GetAimingScore(Vector2 position, SpaceShip opponent)
        {
            var opponentShielHhullScore = opponent.DefenseSystem.ShildLevel * 0.5 + opponent.DefenseSystem.HullLevel * 0.5;
            var distanceScore = Vector2.Distance(position, opponent.Position) / ShortRangeScanDistance;

            return  (1 - opponentShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
        }

    }
}
