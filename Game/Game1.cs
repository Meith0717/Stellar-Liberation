﻿// Game1.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
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
        private SpriteBatch mSpriteBatch;
        private bool IAmActive;
        private bool IsSafeTotart;

        public Game1()
        {
            Content.RootDirectory = "newContent";
            GraphicsManager = new(this);
            mInputManager = new();
            ResolutionManager = new(GraphicsManager);
            PersistanceManager = new();
            mContentLoader = new(Content);

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
            LayerManager.AddLayer(new LoadingLayer(this));
            ResolutionManager.Apply("800x480");
        }

        protected override void LoadContent()
        {
            PersistanceManager.Load<GameSettings>(PersistanceManager.SettingsSaveFilePath, (s) => Settings = s, (_) => Settings = new());
            SoundEffectManager.Instance.SetVolume(Settings.MasterVolume, Settings.SoundEffectsVolume);
            MusicManager.Instance.SetVolume(Settings.MasterVolume, Settings.MusicVolume);

            mContentLoader.LoadEssenzialContent();
            mContentLoader.LoadContentAsync(() => IsSafeTotart = true, (ex) => throw ex);

            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
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
                InputState inputState = mInputManager.Update(gameTime);
                inputState.DoAction(ActionType.ToggleFullscreen, () => ResolutionManager.ToggleFullscreen(Settings.Resolution));
                LayerManager.Update(gameTime, inputState);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            LayerManager.Draw(mSpriteBatch);
            base.Draw(gameTime);
        }

        public void ActivateMyGame(object sendet, EventArgs args) => IAmActive = true;

        public void DeactivateMyGame(object sendet, EventArgs args) => IAmActive = false; //TODO
    }
}