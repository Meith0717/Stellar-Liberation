// Star.cs 
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
        private readonly Light mLight;
        public float Mass => float.PositiveInfinity;

        public Star(float textureScale, int temperature, Color color)
            : base(Vector2.Zero, GameSpriteRegistries.star, textureScale, 1)
        {
            Kelvin = temperature;
            TextureColor = color;
            mLight = new PointLight
            {
                Radius = BoundedBox.Radius,
                ShadowType = ShadowType.Solid
            };
        }

        public override void Initialize(GameLayer gameLayer)
        {
            gameLayer.Penumbra.Lights.Add(mLight);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            base.Update(gameTime, inputState, scene);
            mLight.Scale = new Vector2(3000000);
            mLight.Position = Position;
            mLight.Color = TextureColor * .5f;
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 1.5f, Rotation, 0, TextureColor);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
