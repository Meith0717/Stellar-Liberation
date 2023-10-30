﻿// SensorArray.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems
{
    public class SensorArray
    {
        public List<GameObject> AstronomicalObjects { get; private set; } = new();
        public List<SpaceShip> SortedSpaceShips { get; private set; } = new();

        public int ScanRadius { get; private set; }

        private float mActualCounter;
        private float mMaxCounter = 500;

        public SensorArray(int scanRadius)
        {
            ScanRadius = scanRadius;
            mActualCounter = ExtendetRandom.Random.Next((int)mMaxCounter);
        }

        public void Update(GameTime gameTime, Vector2 position, PlanetSystem planetSystem, Scene scene)
        {
            AstronomicalObjects.Clear();
            AstronomicalObjects.Add(planetSystem.Star);
            AstronomicalObjects.AddRange(planetSystem.Planets);
            SortedSpaceShips = scene.GetObjectsInRadius<SpaceShip>(position, ScanRadius);
        }
    }
}
