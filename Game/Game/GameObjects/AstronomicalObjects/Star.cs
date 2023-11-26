// Star.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    [Collidable]
    public class Star : GameObject2D
    {
        [JsonProperty]
        public Color LightColor { get; private set; }

        public Star(Vector2 position, string textureId, float textureScale, Color starColor)
            : base(position, textureId, textureScale, 1)
        {
            LightColor = starColor;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, scene);
            AddToSpatialHashing(scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(TextureRegistries.starLightAlpha.Name, Position, TextureOffset, TextureScale * 2f, Rotation, 3, LightColor);
        }
    }
}
