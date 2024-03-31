﻿// Star.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

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

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            scene.SpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            base.Update(gameTime, inputState, scene);
            scene.SpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 1.5f, Rotation, 0, TextureColor);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
