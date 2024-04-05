// IGameObject2D.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public interface IGameObject
    {
        void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState);
        void Draw(GameState gameState, GameLayer scene);
    }
}
