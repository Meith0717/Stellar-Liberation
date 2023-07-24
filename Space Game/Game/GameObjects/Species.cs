using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Game.GameObjects.Spacecraft.ScienceShip;
using CelestialOdyssey.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects
{
    [Serializable]
    public class Species
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
        public List<SpaceShip> Ships { get; private set; } = new();

        public Species(string name)
        {
            Name = name;
        }

        public void SpawnCargo(Vector2 position) 
        {
            Cargo ship = new(position);
            Ships.Add(ship);
        }

        public void SpawnScience(Vector2 position)
        {
            ScienceShip ship = new(position);
            Ships.Add(ship);
        }

        public void DestroyShip(SpaceShip ship)
        {
            Ships.Remove(ship);
        }
    }
}
