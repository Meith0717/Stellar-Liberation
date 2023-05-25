﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using Galaxy_Explovive.Game.Layers;

namespace Galaxy_Explovive
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Local Classes
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;
        private readonly TextureManager mTextureManager;

        // Global Classes
        private LayerManager mLayerManager;
        private readonly SoundManager mSoundManager;
        private SpriteBatch mSpriteBatch;

        // Window attributes
        private int mWidth;
        private int mHeight;
        private bool mIsFullScreen;
        private bool mResulutionWasResized;

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += delegate { mResulutionWasResized = true; };

            mGraphicsManager = new GraphicsDeviceManager(this);
            mInputManager = new InputManager();
            mSoundManager = new SoundManager();
            mTextureManager = TextureManager.Instance;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            Globals.GraphicsDevice = GraphicsDevice;

            // Layer Manager
            mLayerManager = new LayerManager(this);
            mLayerManager.AddLayer(new GameLayer(mLayerManager, mSoundManager));
            mLayerManager.AddLayer(new HudLayer(mLayerManager, mSoundManager));

            // Cursor Stuff
            MouseCursor cursor = MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 0, 0);
            Mouse.SetCursor(cursor);
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            // setup texture manager
            mTextureManager.SetContentManager(Content);
            mSoundManager.SetContentManager(Content);
            mTextureManager.SetSpriteBatch(mSpriteBatch);

            mSoundManager.LoadSoundEffects("hit", "SoundEffects/hit");
            mSoundManager.CreateSoundEffectInstances();

            // Load Star Textures
            mTextureManager.LoadTexture("StarLightAlpha", "GameObjects/Stars/StarLightAlpha");
            mTextureManager.LoadTexture("M", "GameObjects/Stars/M");
            mTextureManager.LoadTexture("K", "GameObjects/Stars/K");
            mTextureManager.LoadTexture("G", "GameObjects/Stars/G");
            mTextureManager.LoadTexture("F", "GameObjects/Stars/F");
            mTextureManager.LoadTexture("A", "GameObjects/Stars/A");
            mTextureManager.LoadTexture("B", "GameObjects/Stars/B");
            mTextureManager.LoadTexture("O", "GameObjects/Stars/O");
            mTextureManager.LoadTexture("BH", "GameObjects/Stars/BH");

            // Load Planet Textures
            string planetPath = "GameObjects/Planets/";
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture($"cold{i}", $"{planetPath}cold{i}"); }
            for (int i = 1; i <= 6; i++) { mTextureManager.LoadTexture($"dry{i}", $"{planetPath}dry{i}"); }
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture($"gas{i}", $"{planetPath}gas{i}"); }
            for (int i = 1; i <= 6; i++) { mTextureManager.LoadTexture($"stone{i}", $"{planetPath}stone{i}"); }
            for (int i = 1; i <= 8; i++) { mTextureManager.LoadTexture($"terrestrial{i}", $"{planetPath}terrestrial{i}"); }
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture($"warm{i}", $"{planetPath}warm{i}"); }
            mTextureManager.LoadTexture("planetShadow", "GameObjects/Planets/planetShadow");

            // Load CrossHair
            mTextureManager.LoadTexture("crossHair1", "GameObjects/crossHair/crossHair1");
            mTextureManager.LoadTexture("crossHair2", "GameObjects/crossHair/crossHair2");

            // Load other STuff
            mTextureManager.LoadTexture("gameBackground", "gameBackground");
            mTextureManager.LoadTexture("gameBackgroundParlax1", "gameBackgroundParlax");
            mTextureManager.LoadTexture("gameBackgroundParlax2", "gameBackgroundParlax2");
            mTextureManager.LoadTexture("ship", "GameObjects/Ships/ship");
            mTextureManager.LoadTexture("shipSekect", "GameObjects/Ships/shipSekect");
            mTextureManager.LoadTexture("warpAnimation", "GameObjects/Ships/warpAnimation");
            mTextureManager.LoadTexture("systemState", "GameObjects/systemState");
            mTextureManager.LoadTexture("cursor", "cursor");
            mTextureManager.LoadTexture("transparent", "GameObjects/transparent");
            mTextureManager.LoadTexture("projectile", "GameObjects/projectile");
            mTextureManager.LoadTexture("spaceStation", "GameObjects/spaceStation");

            // Ui
            mTextureManager.LoadTexture("Layer", "UserInterface/Layer/layer");
            mTextureManager.LoadTexture("Circle", "UserInterface/Layer/circle");
            mTextureManager.LoadTexture("menueButton", "UserInterface/menueButton");
            mTextureManager.LoadTexture("buttonExitgame", "UserInterface/buttonExitgame");
            mTextureManager.LoadTexture("buttonContinue", "UserInterface/buttonContinue");

            // game fonts
            mTextureManager.LoadSpriteTexture("text", "fonts/text");
            mTextureManager.LoadSpriteTexture("title", "fonts/title");
            mTextureManager.LoadSpriteTexture("smal", "fonts/smal");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
            // handle window resize
            if (mResulutionWasResized)
            {
                mLayerManager.OnResolutionChanged();
            }
            InputState inputState = mInputManager.Update(gameTime);
            if (inputState.mActionList.Contains(ActionType.ToggleFullscreen)) { ToggleFullscreen(); }
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
        private void ToggleFullscreen()
        {
            Action action = mIsFullScreen ? UnSetFullscreen : SetFullscreen;
            mIsFullScreen = !mIsFullScreen;
            action();
        }

        private void SetFullscreen()
        {
            mWidth = Window.ClientBounds.Width;
            mHeight = Window.ClientBounds.Height;

            mGraphicsManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            mGraphicsManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            mGraphicsManager.IsFullScreen = true;
            mGraphicsManager.ApplyChanges();
        }

        private void UnSetFullscreen()
        {
            mGraphicsManager.PreferredBackBufferWidth = mWidth;
            mGraphicsManager.PreferredBackBufferHeight = mHeight;
            mGraphicsManager.IsFullScreen = false;
            mGraphicsManager.ApplyChanges();
        }
    }
}