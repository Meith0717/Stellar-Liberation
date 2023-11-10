﻿// Game1.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ConfigReader;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Layers;

namespace StellarLiberation
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Local Classes
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;

        // Global Classes
        private SpriteBatch mSpriteBatch;
        public LayerManager mLayerManager;
        public readonly Serialize mSerialize;

        // Window attributes
        private bool mResulutionWasResized;

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += delegate { mResulutionWasResized = true; };

            mInputManager = new InputManager();
            mGraphicsManager = new GraphicsDeviceManager(this);
            mSerialize = new Serialize();

            mGraphicsManager.PreferredBackBufferWidth = 1920;
            mGraphicsManager.PreferredBackBufferHeight = 1080;
            ToggleFullscreen();
        }

        protected override void Initialize()
        {
            base.Initialize();
            mLayerManager = new LayerManager(this, GraphicsDevice, mSerialize);
            mLayerManager.AddLayer(new MainMenueLayer());
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // setup texture manager
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
            TextureManager.Instance.LoadTextureRegistries(Content, Registries.GetRegistryList<TextureRegistries>());
            TextureManager.Instance.LoadFontRegistries(Content, Registries.GetRegistryList<FontRegistries>());

            // setup sound manager
            SoundManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<SoundRegistries>());
            SoundManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<MusicRegistries>());
            SoundManager.Instance.CreateSoundEffectInstances();



            Configs.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) Exit();
            if (mResulutionWasResized) mLayerManager.OnResolutionChanged();

            InputState inputState = mInputManager.Update();
            inputState.DoAction(ActionType.ToggleFullscreen, ToggleFullscreen);
            mLayerManager.Update(gameTime, inputState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            mLayerManager.Draw(mSpriteBatch);
            base.Draw(gameTime);
        }

        // Some Stuff
        private void ToggleFullscreen() =>  mGraphicsManager.ToggleFullScreen();


        public void SetCursor(Registry registry)
        {
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
        }
    }
}