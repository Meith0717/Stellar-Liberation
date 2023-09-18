using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class GameLayer : SceneLayer
    {
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonIgnore] public PlanetSystem ActualPlanetSystem;
        [JsonProperty] public readonly Map Map;
        [JsonProperty] public readonly Player Player;

        public GameLayer() : base(1000000, false, 0.001f, 1f, false)
        {
            // Build and generate map
            Map = new();
            Map.Generate(this);

            // Set start planetsystem
            ActualPlanetSystem = Map.mPlanetSystems.First();

            // Build and place player to startsystem
            Player = new(Map.GetSectorPosition(ActualPlanetSystem.Position));
            Camera.SetPosition(Player.Position);

            // Build and set parllax manager
            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0.05f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2, 0.2f));

            // Play bg music
            SoundManager.Instance.PlaySound(ContentRegistry.bgMusicGame, 1.2f, false, true, true);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (!FrustumCuller.VectorOnScreenView(inputState.mMousePosition) && inputState.mMouseActions.Count > 0) Pause();

            // Update Stuff
            Player.Update(gameTime, inputState, this);
            base.Update(gameTime, inputState);
            Map.Update(gameTime, inputState, this);
            mParllaxManager.Update(Camera.Movement);

            // Get Inputs
            inputState.DoAction(ActionType.ToggleMap, ToggleMapView);
            inputState.DoAction(ActionType.ESC, Pause);

            // Some other stuff
            ActualPlanetSystem = Map.GetActualPlanetSystem(Player);
            if (ActualPlanetSystem is null) ShowExitingSystemWarning();
        }

        public override void DrawOnWorld() { ; }

        public override void DrawOnScreen() { mParllaxManager.Draw(); }

        public override void Destroy() { ; }

        public override void OnResolutionChanged() { mParllaxManager.OnResolutionChanged(mGraphicsDevice); }

        // Other Stuff
        public void ToggleMapView()
        {
            var mapLayer = new MapLayer(this);
            mGame1.SetCursor(ContentRegistry.cursor);
            switch (ActualPlanetSystem)
            {
                case null:
                    mapLayer.Camera.SetPosition(Map.GetMapPosition(Player.Position));
                    break;
                case not null:
                    mapLayer.Camera.SetPosition(ActualPlanetSystem.Position);
                    break;
            }

            mLayerManager.AddLayer(mapLayer);
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer());
        }        

        private void ShowExitingSystemWarning() { }
    }
}
