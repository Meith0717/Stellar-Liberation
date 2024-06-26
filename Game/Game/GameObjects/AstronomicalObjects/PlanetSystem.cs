// Planetsystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class Planetsystem : GameObject, IGameObject
    {
        [JsonProperty] public readonly PlanetsystemState PlanetsystemState;
        [JsonIgnore] private readonly Sector mSector;

        public Planetsystem(PlanetsystemState planetsystemState)
            : base(planetsystemState.MapPosition, "star", .1f, 1)
        {
            TextureColor = planetsystemState.Star.TextureColor;
            PlanetsystemState = planetsystemState;
            mSector = new(Position - (new Vector2(MapFactory.MapScale) / 2), MapFactory.MapScale, MapFactory.MapScale);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            mSector.Update(PlanetsystemState.Occupier);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            var color = TextureColor;

            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw("starLightAlpha", Position, TextureOffset, TextureScale * 2f, Rotation, 0, color);

            mSector.Draw();
        }

    }
}
