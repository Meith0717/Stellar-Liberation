// SpaceShipManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.SpaceShipManagement
{
    public enum ShipType { EnemyBattleShip, EnemyCorvette, EnemyFighter, EnemyCarrior }

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
                case ShipType.EnemyCarrior:
                    AddObj(new Enemys.Carrior(position) { ActualPlanetSystem = planetSystem });
                    break;
            }
        }
    }
}
