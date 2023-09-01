using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
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
        [JsonIgnore] private bool mShowMap;

        [JsonProperty] public readonly Map Map = new();
        [JsonProperty] public readonly Player Player;
        [JsonProperty] public PlanetSystem ActualPlanetSystem;

        public GameLayer() : base(1000000, false, 0.001f, 1, false)
        {
            Map.Generate(this);
            ActualPlanetSystem = Map.GetRandomSystem();
            Player = new(Map.GetSectorPosition(ActualPlanetSystem.Position));
            Camera.SetPosition(Player.Position);

            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);

            Player.Update(gameTime, inputState, this);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);

            ActualPlanetSystem = Map.GetActualPlanetSystem(Player);

            if (inputState.mActionList.Contains(ActionType.ToggleMap)) ToggleMapView();
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

        private void ToggleMapView()
        {
            mShowMap = !mShowMap;
            if (!mShowMap) return;
            mMapLayer ??= new(this);

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
    }
}
