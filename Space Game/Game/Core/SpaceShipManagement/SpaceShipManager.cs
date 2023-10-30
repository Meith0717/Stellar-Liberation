// SpaceShipManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement
{
    public enum ShipType { EnemyBattleShip, EnemyCorvette, EnemyFighter, Carrior }

    public class SpaceShipManager : GameObjectManager
    {
        public void AddRange(List<SpaceShip> spaceShips) => base.AddRange(spaceShips);

        public void Spawn(PlanetSystem planetSystem, Vector2 position, ShipType type)
        {
            switch (type)
            {
                case ShipType.EnemyBattleShip:
                    AddObj(new Enemys.BattleShip(position) { ActualPlanetSystem = planetSystem });
                    break;
                case ShipType.EnemyCorvette:
                    AddObj(new Enemys.Bomber(position) { ActualPlanetSystem = planetSystem });
                    break;
                case ShipType.EnemyFighter:
                    AddObj(new Enemys.Fighter(position) { ActualPlanetSystem = planetSystem });
                    break;
                case ShipType.Carrior:
                    AddObj(new Enemys.Carrior(position) { ActualPlanetSystem = planetSystem });
                    break;
            }
        }
    }
}
