// Game1.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Layers;
using StellarLiberation.Game.Layers.MenueLayers;
using System;

namespace StellarLiberation
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GameSettings Settings { get; private set; }
        public LayerManager LayerManager { get; private set; }
        public readonly GraphicsDeviceManager GraphicsManager;
        public readonly PersistanceManager PersistanceManager;
        public readonly ResolutionManager ResolutionManager;

        private readonly InputManager mInputManager;
        private readonly ContentLoader mContentLoader;
        private readonly FrameCounter mFrameCounter;
        private SpriteBatch mSpriteBatch;
        private bool IAmActive;
        private bool IsSafeTotart;

        public Game1()
        {
            Content.RootDirectory = "Content";
            GraphicsManager = new(this);
            mInputManager = new();
            ResolutionManager = new(GraphicsManager);
            PersistanceManager = new();
            mContentLoader = new(Content);
            mFrameCounter = new(200);

            Activated += ActivateMyGame;
            Deactivated += DeactivateMyGame;
            IAmActive = false;

            // Window properties
            IsMouseVisible = true;
            Window.AllowAltF4 = false;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            LayerManager = new(this);
            LayerManager.AddLayer(new LoadingLayer(this, mContentLoader));
            ResolutionManager.Apply("1600x900");
            // GraphicsManager.ToggleFullScreen();
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
            TextureManager.Instance.SetGraphicsDevice(GraphicsDevice);

            PersistanceManager.Load<GameSettings>(PersistanceManager.SettingsSaveFilePath, (s) => Settings = s, (_) => Settings = new());
            SoundEffectManager.Instance.SetVolume(Settings.MasterVolume, Settings.SoundEffectsVolume);
            MusicManager.Instance.SetVolume(Settings.MasterVolume, Settings.MusicVolume);

            mContentLoader.LoadEssenzialContent();
            mContentLoader.LoadContentAsync(() => IsSafeTotart = true, (ex) => throw ex);
        }

        private void StartMainMenue()
        {
            GraphicsManager.PreferMultiSampling = true;
            GraphicsManager.SynchronizeWithVerticalRetrace = Settings.Vsync;
            if (IsFixedTimeStep = long.TryParse(Settings.RefreshRate, out long rate))
                TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / rate);
            GraphicsManager.ApplyChanges();

            LayerManager.PopLayer();
            LayerManager.AddLayer(new MainMenueLayer(this));

            IsSafeTotart = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsSafeTotart)
                StartMainMenue();

            MusicManager.Instance.Update();
            if (ResolutionManager.WasResized)
                LayerManager.OnResolutionChanged();
            if (IAmActive)
            {
                mFrameCounter.Update(gameTime);
                InputState inputState = mInputManager.Update(gameTime);
                inputState.DoAction(ActionType.ToggleFullscreen, () => ResolutionManager.ToggleFullscreen(Settings.Resolution));
                LayerManager.Update(gameTime, inputState);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            mFrameCounter.UpdateFrameCouning();
            GraphicsDevice.Clear(Color.Transparent);
            LayerManager.Draw(mSpriteBatch);
            mSpriteBatch.Begin();
            TextureManager.Instance.DrawString("consola", new Vector2(1, 1), $"{MathF.Round(mFrameCounter.CurrentFramesPerSecond)} fps", 0.1f, Color.White);
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

        public void ActivateMyGame(object sendet, EventArgs args) => IAmActive = true;

        public void DeactivateMyGame(object sendet, EventArgs args) => IAmActive = false;
    }
}