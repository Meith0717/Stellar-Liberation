// Star.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public class Star : GameObject2D, ICollidable
    {
        public readonly int Kelvin;
        public float Mass => float.PositiveInfinity;

        public Star(float textureScale, int temperature, Color color)
            : base(Vector2.Zero, GameSpriteRegistries.star, textureScale, 1)
        {
            Kelvin = temperature;
            TextureColor = color;
        }

        public override void Initialize(GameLayer gameLayer)
        {
            var light = new PointLight
            {
                Scale = new Vector2(100000),
                Color = TextureColor,
                Intensity = .5f,
                Radius = MaxTextureSize * TextureScale,
                Position = Position,
                ShadowType = ShadowType.Solid
            };
            gameLayer.Penumbra.Lights.Add(light);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
