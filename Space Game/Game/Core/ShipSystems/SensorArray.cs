using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems
{
    public class SensorArray
    {

        public List<GameObject> AstronomicalObjects { get; private set; } = new();
        public List<SpaceShip> SortedSpaceShips { get; private set; } = new();
        public int ScanRadius { get; private set; }

        private float mActualCounter;
        private float mMaxCounter = 1000;

        public SensorArray(int scanRadius, int scanCoolDown) 
        { 
            ScanRadius = scanRadius;
            mMaxCounter = scanCoolDown;
            mActualCounter = Utility.Utility.Random.Next(1000);
        }

        public void Update(GameTime gameTime, Vector2 position, PlanetSystem planetSystem, Scene scene)
        {
            mActualCounter += gameTime.TotalGameTime.Milliseconds;
            if (mActualCounter < mMaxCounter) return;
            mActualCounter = 0;
            AstronomicalObjects.Clear();
            AstronomicalObjects.Add(planetSystem.Star);
            AstronomicalObjects.AddRange(planetSystem.Planets);
            SortedSpaceShips = scene.GetObjectsInRadius<SpaceShip>(position, ScanRadius);
        }
    }
}
