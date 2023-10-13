// SpaceShipManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using System.Collections.Generic;
using System.Numerics;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement
{
    public enum ShipType { EnemyBattleShip, EnemyCorvette, EnemyFighter }

    public class SpaceShipManager : GameObjectManager
    {
        public void AddRange(List<SpaceShip> spaceShips) => base.AddRange(spaceShips);

        public void Spawn(PlanetSystem planetSystem, ShipType type)
        {
            switch (type)
            {
                case ShipType.EnemyBattleShip:
                    AddObj(new EnemyBattleShip(Vector2.Zero) { ActualPlanetSystem = planetSystem });
                    break;
                case ShipType.EnemyCorvette:
                    AddObj(new EnemyCorvette(Vector2.Zero) { ActualPlanetSystem = planetSystem });
                    break;
                case ShipType.EnemyFighter:
                    AddObj(new EnemyFighter(Vector2.Zero) { ActualPlanetSystem = planetSystem });
                    break;
                default:
                    break;
            }
        }
    }
}
