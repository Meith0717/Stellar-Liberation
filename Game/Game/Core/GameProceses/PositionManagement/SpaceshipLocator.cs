// SpaceshipTracer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    public class SpaceshipLocator
    {
        private readonly Dictionary<Spaceship, PlanetSystem> mShipSystemTracer = new();

        public void Update(List<PlanetSystem> planetSystems)
        {
            mShipSystemTracer.Clear();
            foreach (var planetSystem in planetSystems)
            {
                foreach (var spaceship in planetSystem.GameObjects.OfType<Spaceship>())
                {
                    mShipSystemTracer.Add(spaceship, planetSystem);
                }
            }
        }

        public PlanetSystem Locate(Spaceship spaceship)
        {
            if (!mShipSystemTracer.TryGetValue(spaceship, out var planetSystem))
                planetSystem = null;
            return planetSystem;
        }

        public void ChangeSystem(Spaceship spaceship, PlanetSystem system) 
            => mShipSystemTracer[spaceship] = system;
    }
}
