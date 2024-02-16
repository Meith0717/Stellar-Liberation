// Star.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    [Collidable]
    public class Star : GameObject2D, ICollidable
    {
        [JsonProperty] public readonly int Kelvin;
        public float Mass => float.PositiveInfinity;

        public Star(float textureScale, int temperature, Color color)
            : base(Vector2.Zero, GameSpriteRegistries.star, textureScale, 1)
        {
            Kelvin = temperature;
            TextureColor = color;
        }


        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
