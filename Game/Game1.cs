// Game1.cs 
// Copyright (c) 2023 Thierry Meiers 
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
        private readonly ResolutionManager mResolutionManager;
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;
        private readonly PersistanceManager mPersistanceManager;
        private GameSettings mGameSettings;
        private SpriteBatch mSpriteBatch;
        private LayerManager mLayerManager;
        private bool IAmActive;

        public Game1()
        {
            Content.RootDirectory = "Content";
            mGraphicsManager = new(this);
            mInputManager = new();
            mResolutionManager = new(mGraphicsManager);
            mPersistanceManager = new();

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
            mLayerManager = new(this, GraphicsDevice, mPersistanceManager, mResolutionManager, mGameSettings);
            mLayerManager.AddLayer(new LoadingLayer());
            mResolutionManager.Apply(mGameSettings.Resolution);
            mResolutionManager.ToggleFullscreen(mGameSettings.Resolution);
        }

        protected override void LoadContent()
        {
            mPersistanceManager.Load<GameSettings>(PersistanceManager.SettingsSaveFilePath, (s) => mGameSettings = s, (_) => mGameSettings = new());

            SoundEffectManager.Instance.SetVolume(mGameSettings.MasterVolume, mGameSettings.SoundEffectsVolume);
            MusicManager.Instance.SetVolume(mGameSettings.MusicVolume, mGameSettings.MusicVolume);

            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.PreLoad(Content, mSpriteBatch);
            ContentLoader.LoadAsync(Content, mSpriteBatch, () => { mLayerManager.PopLayer(); mLayerManager.AddLayer(new MainMenueLayer()); }, (ex) => throw ex);
        }

        protected override void Update(GameTime gameTime)
        {
            if (IAmActive)
            {
                InputState inputState = mInputManager.Update();
                MusicManager.Instance.Update();
                inputState.DoAction(ActionType.ToggleFullscreen, () => mResolutionManager.ToggleFullscreen(mGameSettings.Resolution));
                inputState.DoAction(ActionType.IncreaseScaling, () => mResolutionManager.UiScaling += 0.1f);
                inputState.DoAction(ActionType.DecreaseScaling, () => mResolutionManager.UiScaling -= 0.1f);
                mLayerManager.Update(gameTime, inputState);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            mLayerManager.Draw(mSpriteBatch);
            base.Draw(gameTime);
        }

        public void ActivateMyGame(object sendet, EventArgs args) => IAmActive = true;

        public void DeactivateMyGame(object sendet, EventArgs args) => IAmActive = false;

        public void SetCursorTexture(Registry registry) => Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
    }
}