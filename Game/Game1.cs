// Game1.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
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
        private SpriteBatch mSpriteBatch;
        private bool IAmActive;

        public Game1()
        {
            Content.RootDirectory = "Content";
            GraphicsManager = new(this);
            mInputManager = new();
            ResolutionManager = new(GraphicsManager);
            PersistanceManager = new();

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
            ResolutionManager.Apply(Settings.Resolution);

            GraphicsManager.PreferMultiSampling = true;
            GraphicsManager.SynchronizeWithVerticalRetrace = Settings.Vsync;
            if (IsFixedTimeStep = long.TryParse(Settings.RefreshRate, out long rate))
                TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / rate);
            GraphicsManager.ApplyChanges();
        }

        protected override void LoadContent()
        {
            PersistanceManager.Load<GameSettings>(PersistanceManager.SettingsSaveFilePath, (s) => Settings = s, (_) => Settings = new());

            SoundEffectManager.Instance.SetVolume(Settings.MasterVolume, Settings.SoundEffectsVolume);
            MusicManager.Instance.SetVolume(Settings.MasterVolume, Settings.MusicVolume);

            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.PreLoad(Content, mSpriteBatch);
            ContentLoader.LoadAsync(Content, mSpriteBatch, () => { LayerManager.PopLayer(); LayerManager.AddLayer(new MainMenueLayer(this)); }, (ex) => throw ex);
        }

        protected override void Update(GameTime gameTime)
        {
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

        public void DeactivateMyGame(object sendet, EventArgs args) => IAmActive = true; //TODO

        public void SetCursorTexture(Registry registry) => Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
    }
}