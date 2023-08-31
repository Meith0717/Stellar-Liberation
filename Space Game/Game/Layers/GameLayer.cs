using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    internal class GameLayer : SceneLayer
    {
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonIgnore] private readonly Map map = new();
        [JsonProperty] public Player mMainShip;

        public GameLayer() : base(1000000)
        {
            map.Generate(this);
            mMainShip = new(new(map.Width / 2, map.Height / 2));
            Camera.SetPosition(mMainShip.Position);

            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0.05f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mMainShip.Update(gameTime, inputState, this);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void DrawOnScene() { map.DrawSectores(this); }

        public override void Destroy() { ; }
        public override void OnResolutionChanged()
        {
            mParllaxManager.OnResolutionChanged(mGraphicsDevice);
        }

    }
}
