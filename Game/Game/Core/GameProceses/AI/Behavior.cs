// Behavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.Core.GameProceses.AI
{
    public abstract class Behavior
    {
        public abstract double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene);
        public abstract void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene);
        public abstract void Reset(SpaceShip spaceShip);
    }
}
