// PlanetSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class Planetsystem : GameObject2D
    {
        public readonly PlanetsystemState mPlanetsystemState;
        private readonly Sector mSector;

        public Planetsystem(PlanetsystemState planetsystemState)
            : base(planetsystemState.MapPosition, GameSpriteRegistries.star, .1f, 1)
        {
            TextureColor = planetsystemState.Star.TextureColor;
            mPlanetsystemState = planetsystemState;
            mSector = new(Position - (new Vector2(MapFactory.MapScale) / 2), MapFactory.MapScale, MapFactory.MapScale);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            mSector.Update(mPlanetsystemState.Occupier);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            var color = TextureColor;

            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, color);

            mSector.Draw();
        }

    }
}
