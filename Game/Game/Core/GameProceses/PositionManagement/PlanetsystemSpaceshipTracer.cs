// SpatialGameObject2DManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    [Serializable]
    public class PlanetsystemSpaceshipTracer
    {
        [JsonProperty] private readonly Dictionary<int, List<Spaceship>> mSystemShipTracer = new();
        [JsonProperty] private readonly Dictionary<int, PlanetSystem> mShipSystemTracer = new();


        public void AddSpaceShip(PlanetSystem planetSystem, Spaceship spaceship)
        {
            if (!mSystemShipTracer.TryGetValue(planetSystem.Seed, out var spaceships))
                mSystemShipTracer[planetSystem.Seed] = spaceships = new();
            spaceships.Add(spaceship);
            mShipSystemTracer[spaceship.ID] = planetSystem;
        }

        public void ChangePlanetSystem(PlanetSystem destination, Spaceship spaceship)
        {
            var old = LocateSpaceShip(spaceship);
            if (!mSystemShipTracer.TryGetValue(old.Seed, out var spaceships)) return;
            spaceships.Remove(spaceship);
            AddSpaceShip(destination, spaceship);
            mShipSystemTracer[spaceship.ID] = destination;
        }

        public void ChangePlanetSystem(PlanetSystem actual, PlanetSystem destination, Spaceship spaceship)
        {
            if (!mSystemShipTracer.TryGetValue(actual.Seed, out var spaceships)) return;
            spaceships.Remove(spaceship);
            AddSpaceShip(destination, spaceship);
        }

        public PlanetSystem LocateSpaceShip(Spaceship spaceship)
        {
            if (!mShipSystemTracer.TryGetValue(spaceship.ID, out var planetSystem))
                planetSystem = null;
            return planetSystem;
        }

        public List<Spaceship> GetSpaceshipsOfPlanetSystem(PlanetSystem planetSystem) 
        {
            if (!mSystemShipTracer.TryGetValue(planetSystem.Seed, out var spaceships))
                return new();
            return spaceships;
        }

    }
}
