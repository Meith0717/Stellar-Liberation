// Behavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;

namespace StellarLiberation.Game.Core.AI
{
    public abstract class Behavior
    {
        public abstract double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene);
        public abstract void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene);
        public abstract void Reset(SpaceShip spaceShip);
    }
}
