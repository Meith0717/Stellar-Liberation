// SensorArray.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems
{
    public class SensorArray
    {
        // Scan results
        public List<GameObject2D> ObjInDistance { get; private set; } = new();
        public readonly List<GameObject2D> AstronomicalObjects = new();
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
            ObjInDistance = scene.SpatialHashing.GetObjectsInRadius<GameObject2D>(position, ShortRangeScanDistance);
            ObjInDistance.AddRange(AstronomicalObjects);

            var opponents = new List<SpaceShip>(); 
            switch (opponent)
            {
                case Factions.Enemys:
                    var enemis = (IEnumerable<SpaceShip>)ObjInDistance.OfType<Enemy>();
                    opponents = enemis.ToList();
                    break;
                case Factions.Allies:
                    var allies = (IEnumerable<SpaceShip>)ObjInDistance.OfType<Player>();
                    opponents = allies.ToList();
                    break;
            }

            AimingShip =  GetAimingShip(position, opponents);
            DistanceToAimingShip = AimingShip is null ? float.PositiveInfinity : Vector2.Distance(position, AimingShip.Position);
        }

        private SpaceShip GetAimingShip(Vector2 position, List<SpaceShip> opponents)
        {
            double GetAimingScore(Vector2 position, SpaceShip opponent)
            {
                var opponentShielHhullScore = opponent.DefenseSystem.ShieldPercentage * 0.5 + opponent.DefenseSystem.HullPercentage * 0.5;
                var distanceScore = Vector2.Distance(position, opponent.Position) / ShortRangeScanDistance;

                return (1 - opponentShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
            }

            PriorityQueue<SpaceShip, double> q = new();
            foreach (var opponent in opponents) q.Enqueue(opponent, -GetAimingScore(position, opponent));
            q.TryDequeue(out var spaceShip, out var _);
            return spaceShip;
        }
    }
}
