// Game1.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.ConfigReader;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Persistance;
using CelestialOdyssey.Game.Layers.Scenes;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CelestialOdyssey
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

            mGraphicsManager.PreferredBackBufferWidth = (int)(900 * 1.77777777778f);
            mGraphicsManager.PreferredBackBufferHeight = 900;
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
            foreach (Registry registry in ContentRegistry.IterateThroughRegistries())
            {
                switch (registry.RegistryType)
                {
                    case RegistryType.Texture:
                        TextureManager.Instance.LoadTexture(Content, registry.Name, registry.FilePath);
                        break;
                    case RegistryType.Font:
                        TextureManager.Instance.LoadFont(Content, registry.Name, registry.FilePath);
                        break;
                    case RegistryType.Sound:
                        SoundManager.Instance.LoadSoundEffects(Content, registry.Name, registry.FilePath);
                        break;
                }
            }
            SoundManager.Instance.CreateSoundEffectInstances();
            SetCursor(ContentRegistry.cursor);

            // game fonts
            TextureManager.Instance.LoadFont(Content, "debug", "fonts/debug");
            TextureManager.Instance.LoadFont(Content, "title", "fonts/title");

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