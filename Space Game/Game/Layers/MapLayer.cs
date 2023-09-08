
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);

            foreach (var planetSystems in mGameLayer.Map.mPlanetSystems)
            {
                if (!FrustumCuller.CircleOnWorldView(planetSystems.BoundedBox)) continue;
                planetSystems.CheckIfHasPlayer(mGameLayer.Player);
                planetSystems.Update(gameTime, inputState, this);
                if (planetSystems.LeftPressed) SetPlayerTarget(planetSystems);
            }
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);
            inputState.DoAction(ActionType.ToggleMap, CloseMap);
        }

        private void SetPlayerTarget(PlanetSystem planetSystem) 
        {
            mGameLayer.Player.SetTarget(planetSystem, this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public void CloseMap() 
        { 
            mLayerManager.PopLayer();
            mGame1.SetCursor(ContentRegistry.cursor1);
        }

        public override void Destroy() { ; }

        public override void DrawOnScene() 
        { 
            mGameLayer.Player.DrawPath(mGameLayer, this);
        }

        public override void OnResolutionChanged() { mParllaxManager.OnResolutionChanged(mGraphicsDevice); }
    }
}
