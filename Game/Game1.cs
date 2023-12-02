// Game1.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarLiberation.Core.GameEngine.Content_Management;
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
            mGraphicsManager = new(this);
            mInputManager = new();
            mSerialize = new Serialize();

            // Window properties
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = false;
            Window.ClientSizeChanged += delegate { mResulutionWasResized = true; };
            mGraphicsManager.PreferredBackBufferWidth = 1920;
            mGraphicsManager.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            base.Initialize();
            mLayerManager = new(this, GraphicsDevice, mSerialize);
            mLayerManager.AddLayer(new MainMenueLayer());
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
            TextureManager.Instance.LoadTextureRegistries(Content, Registries.GetRegistryList<TextureRegistries>());
            TextureManager.Instance.LoadFontRegistries(Content, Registries.GetRegistryList<FontRegistries>());
            SoundEffectManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<SoundEffectRegistries>());
            MusicManager.Instance.LoadRegistries(Content, Registries.GetRegistryList<MusicRegistries>());
        }

        protected override void Update(GameTime gameTime)
        {
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

        private void ToggleFullscreen() =>  mGraphicsManager.ToggleFullScreen();

        public void SetCursorTexture(Registry registry) => Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
    }
}