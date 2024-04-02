// ScienceStation.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Layers;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceStations
{
    public class ScienceStation : GameObject2D, ICollidable
    {
        public float Mass => float.PositiveInfinity;

        public ScienceStation(Vector2 position)
            : base(position, GameSpriteRegistries.scienceStation, 1, 20) { }

        public override void Update(GameTime gameTime, InputState inputState, GameState gameState, PlanetsystemState planetsystemState)
        {
            base.Update(gameTime, inputState, gameState, planetsystemState);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
