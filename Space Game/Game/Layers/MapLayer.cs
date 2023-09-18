
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Layers
{
    public class MapLayer : SceneLayer
    {
        private readonly ParllaxManager mParllaxManager = new();
        private readonly GameLayer mGameLayer;

        public MapLayer(GameLayer gameLayer) 
            : base(100, true, 0.5f, 4, true)
        {
            mGameLayer = gameLayer;

            foreach (var planetSystems in mGameLayer.Map.mPlanetSystems)
            {
                planetSystems.AddToSpatialHashing(this);
            }

            mParllaxManager.Add(new(ContentRegistry.gameBackground, 0));
            mParllaxManager.Add(new(ContentRegistry.mapBackground, 0));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);

            foreach (var planetSystems in mGameLayer.Map.mPlanetSystems)
            {
                if (!FrustumCuller.CircleOnWorldView(planetSystems.BoundedBox)) continue;
                planetSystems.CheckIfHasPlayer(mGameLayer.Player);
                planetSystems.UpdateOnMap(gameTime, inputState, this);
                if (planetSystems.LeftPressed) SetPlayerTarget(planetSystems);
            }
            mParllaxManager.Update(Camera.Movement);
            inputState.DoAction(ActionType.ToggleMap, CloseMap);
        }

        private void SetPlayerTarget(PlanetSystem planetSystem) 
        {
            mGameLayer.Player.HyperDrive.SetTarget(planetSystem, this);
        }

        public override void DrawOnScreen() { mParllaxManager.Draw(); }

        public override void DrawOnWorld() { mGameLayer.Player.DrawPath(mGameLayer, this); mGameLayer.Map.DrawSectores(this); }

        public void CloseMap() 
        { 
            mLayerManager.PopLayer();
            mGame1.SetCursor(ContentRegistry.cursor1);
            foreach (var planetSystems in mGameLayer.Map.mPlanetSystems)
            {
                planetSystems.RemoveFromSpatialHashing(this);
            }
        }

        public override void Destroy() { ; }

        public override void OnResolutionChanged() { mParllaxManager.OnResolutionChanged(mGraphicsDevice); }
    }
}
