// SensorArray.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems
{
    public class SensorArray
    {
        public List<GameObject> AstronomicalObjects { get; private set; } = new();
        public List<SpaceShip> SortedSpaceShips { get; private set; } = new();
        public SpaceShip AimingShip { get; private set; }

        public int ScanRadius { get; private set; }

        private float mActualCounter;
        private float mMaxCounter = 500;

        public SensorArray(int scanRadius)
        {
            ScanRadius = scanRadius;
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
            SortedSpaceShips = scene.GetObjectsInRadius<SpaceShip>(position, ScanRadius);

            AimingShip = null;
            switch (opponent)
            {
                case Factions.Enemys:
                    var enemys = SortedSpaceShips.OfType<Enemy>();
                    if (!enemys.Any()) return;
                    AimingShip = enemys.ToList()[0];
                    break;
                case Factions.Allies:
                    var allies = SortedSpaceShips.OfType<Player>();
                    if (!allies.Any()) return;
                    AimingShip = allies.ToList()[0];
                    break;
            }
        }
    }
}
