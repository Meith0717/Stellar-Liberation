using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.Core.Persistance;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class GameLayer : SceneLayer
    {
        [JsonIgnore] private readonly ParllaxManager mParllaxManager = new();
        [JsonIgnore] private MapLayer mMapLayer;

        [JsonProperty] public readonly Map Map = new();
        [JsonProperty] public readonly Player Player;
        [JsonProperty] public PlanetSystem ActualPlanetSystem;

        [JsonIgnore] private Pirate pirate;

        public GameLayer() : base(1000000, false, 0.001f, 0.01f, false)
        {
            Map.Generate(this);
            ActualPlanetSystem = Map.mPlanetSystems[0];
            Player = new(Map.GetSectorPosition(ActualPlanetSystem.Position));
            pirate = new(Utility.GetRandomVector2(Map.GetSectorPosition(ActualPlanetSystem.Position), 1000000));
            Camera.SetPosition(Player.Position);

            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));

            SoundManager.Instance.PlaySound(ContentRegistry.bgMusicGame, 1f, false, true, true);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mGame1.SetCursor(ContentRegistry.cursor1);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Update Stuff
            Player.Update(gameTime, inputState, this);
            pirate.Update(gameTime, inputState, this);
            base.Update(gameTime, inputState);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);

            // Get Inputs
            inputState.DoAction(ActionType.ToggleMap, ToggleMapView);
            inputState.DoAction(ActionType.ESC, Pause);

            // Some other stuff
            ActualPlanetSystem = Map.GetActualPlanetSystem(Player);
            if (ActualPlanetSystem is null) ShowExitingSystemWarning();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void DrawOnScene() { ; }
        public override void Destroy() { ; }
        public override void OnResolutionChanged() { mParllaxManager.OnResolutionChanged(mGraphicsDevice); }
        public void ToggleMapView()
        {
            mMapLayer ??= new(this);
            mGame1.SetCursor(ContentRegistry.cursor);
            switch (ActualPlanetSystem)
            {
                case null:
                    mMapLayer.Camera.SetPosition(Map.GetMapPosition(Player.Position));
                    break;
                case not null:
                    mMapLayer.Camera.SetPosition(ActualPlanetSystem.Position);
                    break;
            }

            mLayerManager.AddLayer(mMapLayer);
        }
        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer());
        }
        private void ShowExitingSystemWarning() { }
    }
}
