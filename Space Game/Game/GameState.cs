 using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Game.GameObjects;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Utility;
using Newtonsoft.Json;

namespace Galaxy_Explovive.Game
{
    public static class GameGlobals
    {
        public static float GameTime { get; set; }
        public static Camera2d Camera { get; set; }
        public static SpatialHashing<GameObject> SpatialHashing { get; set; }
        public static FrustumCuller FrustumCuller { get; set; }
        public static DebugSystem DebugSystem { get; set; }
        public static SoundManager SoundManager { get; set; }
        public static TextureManager TextureManager { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static MyUiMessageManager MessageManager { get; set; }
        public static Vector2 WorldMousePosition { get; set; }
        public static Vector2 ViewMousePosition { get; set; }
        public static InteractiveObject SelectObject { get; set; }
    }

    [Serializable]
    public class GameState
    {
        // Saved Classes
        [JsonProperty] public float GameTime;
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;
        [JsonProperty] private List<Cargo> mShips = new(); // Has to be set to JsonProperty !!!!
        [JsonProperty] public readonly Map mMap;

        // Unsaved Classes
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;

        public GameState(GraphicsDevice graphicsDevice, TextureManager textureManager,
            SoundManager soundManager)
        {
            GameGlobals.SpatialHashing = new(1000);
            GameGlobals.FrustumCuller = new();
            GameGlobals.Camera = new(graphicsDevice);
            GameGlobals.DebugSystem = new();
            GameGlobals.SoundManager = soundManager;
            GameGlobals.TextureManager = textureManager;
            GameGlobals.GraphicsDevice = graphicsDevice;
            mParllaxManager = new();
            mMap = new(3000000, 62500);
            mMap.Generate();
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.15f));
            mParllaxManager.Add(new("gameBackgroundParlax3", 0.2f));
            mParllaxManager.Add(new("gameBackgroundParlax4", 0.25f));
            mShips.Add(new(MyUtility.GetRandomVector2(Vector2.Zero, 0)));
        }

        public void Update(InputState inputState, GameTime gameTime) 
        {
            GameGlobals.ViewMousePosition = inputState.mMousePosition.ToVector2();
            GameGlobals.WorldMousePosition = GameGlobals.Camera.ViewToWorld(GameGlobals.ViewMousePosition);
            GameGlobals.FrustumCuller.Update(GameGlobals.GraphicsDevice, GameGlobals.Camera.ViewToWorld);
            GameGlobals.TextureManager.Update(GameGlobals.Camera.Zoom);
            GameGlobals.GameTime = GameTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            foreach (Cargo c in mShips) { c.UpdateLogik(gameTime, inputState); }
            if (inputState.mActionList.Contains(ActionType.ToggleRayTracing))
            {
                Globals.mRayTracing = !Globals.mRayTracing;
            }
            if (GameGlobals.SelectObject != null) { GameGlobals.SelectObject.SelectActions(inputState); }
            mMap.Update(gameTime, inputState);
            mParllaxManager.Update(GameGlobals.Camera.Movement, GameGlobals.Camera.Zoom);
            GameGlobals.DebugSystem.Update(gameTime, inputState);
            GameGlobals.Camera.Update(gameTime, inputState);
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            GameGlobals.DebugSystem.UpdateFrameCounting();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw(GameGlobals.TextureManager);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, 
                transformMatrix: GameGlobals.Camera.GetViewTransformationMatrix(), 
                samplerState: SamplerState.PointClamp);

            mMap.Draw(GameGlobals.TextureManager);
             foreach (Cargo c in mShips)
            {
                c.Draw(GameGlobals.TextureManager);
            }
            GameGlobals.DebugSystem.TestSpatialHashing(GameGlobals.TextureManager, GameGlobals.SpatialHashing, GameGlobals.WorldMousePosition);
            spriteBatch.End();
        }

        public void ApplyResolution()
        {
            mParllaxManager.OnResolutionChanged(GameGlobals.GraphicsDevice);
        }
    }
}
