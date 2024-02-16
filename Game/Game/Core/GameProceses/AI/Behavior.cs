// Behavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI
{
    public abstract class Behavior
    {
        public abstract double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene);
        public abstract void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene);
        public abstract void Reset(SpaceShip spaceShip);
    }
}
