using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class MainGameLayer : GameLayer
    {
        [JsonProperty] public float GameTime;

        [JsonIgnore] public MapItemsManager ItemsManager = new();

        // Unsaved Classes
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonProperty] private readonly Map map = new();

        public MainGameLayer() : base()
        {
            map.Generate(this);

            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0.05f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();
            base.Draw(spriteBatch);
        }
        public override void DrawOnWorld()
        {
            map.DrawSectores(FrustumCuller.WorldFrustum.ToRectangle(), Camera.Zoom);
        }

        public override void OnResolutionChanged()
        {
            mParllaxManager.OnResolutionChanged(GraphicsDevice);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);
        }

    }
}
