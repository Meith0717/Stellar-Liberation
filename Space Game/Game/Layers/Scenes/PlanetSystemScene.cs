using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Layers.Scenes
{
    internal class PlanetSystemScene : Scene
    {
        private PlanetSystem mPlanetSystem;
        private ParllaxManager mParlaxManager;
        private Player mPlayer;

        public PlanetSystemScene(PlanetSystem planetSystem, Player player) : base(500000, 0.00009f, 1, false)
        {
            mPlanetSystem = planetSystem;
            mPlayer = player;

            mParlaxManager = new();
            mParlaxManager.Add(new(ContentRegistry.gameBackgroundParlax1, .001f));
        }

        public override void Update(GameTime gameTime, InputState inputState, int screenWidth, int screenHeight)
        {
            base.Update(gameTime, inputState, screenWidth, screenHeight);

            mParlaxManager.Update(Camera.Movement);

            mSceneManagerLayer.DebugSystem.CheckForSpawn(mPlanetSystem);
            mPlanetSystem.Update(gameTime, inputState, mSceneManagerLayer, this);
            mPlayer.Update(gameTime, inputState, mSceneManagerLayer, this);
        }

        public override void DrawOnScreen() 
        {
            mParlaxManager.Draw();
        }

        public override void DrawOnWorld() { }

        public override void OnResolutionChanged() { }
    }
}
