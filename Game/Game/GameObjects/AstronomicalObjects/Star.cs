// Star.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class Star : GameObject, IGameObject
    {
        [JsonProperty] public readonly int Kelvin;
        [JsonProperty] public float Mass => float.PositiveInfinity;

        public Star(float textureScale, int temperature, Color color)
            : base(Vector2.Zero, GameSpriteRegistries.star, textureScale, 1)
        {
            Kelvin = temperature;
            TextureColor = color;
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState) => base.Update(gameTime, gameState, planetsystemState);

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 1.5f, Rotation, 0, TextureColor);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
