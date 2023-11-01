// SensorArray.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
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

            switch (opponent)
            {
                case Factions.Enemys:
                    var enemys = SortedSpaceShips.OfType<Enemy>();
                    if (!enemys.Any()) { AimingShip = null; break; }
                    // if (!enemys.Contains(AimingShip)) 
                    AimingShip = enemys.ToList()[0];
                    break;
                case Factions.Allies:
                    var allies = SortedSpaceShips.OfType<Player>();
                    if (!allies.Any()) { AimingShip = null; break; }
                    // if (!allies.Contains(AimingShip))
                    AimingShip = allies.ToList()[0];
                    break;
            }

            DistanceToAimingShip = AimingShip is null ? float.PositiveInfinity : Vector2.Distance(position, AimingShip.Position);
        }
    }
}
