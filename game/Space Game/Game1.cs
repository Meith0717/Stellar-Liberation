using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Game.GameObjects;
using Space_Game.Game.Layers;
using System;
using System.Collections.Generic;

namespace Space_Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager mGraphicsDeviceManager;
        private SpriteBatch mSpriteBatch;
        private InputManager mInputManager;
        private SoundManager mSoundManager;
        private LayerManager mLayerManager;
        private TextureManager mTextureManager;

        private int mWidth;
        private int mHeight;
        private bool mIsFullScreen;

        private bool mResulutionWasResized;

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            mGraphicsDeviceManager = new GraphicsDeviceManager(this);
            mInputManager = new InputManager();
            mSoundManager = new SoundManager();
            mTextureManager = TextureManager.GetInstance();
            Window.ClientSizeChanged += delegate { mResulutionWasResized = true; };
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals.mGraphicsDevice = GraphicsDevice;
            Globals.mContentManager = Content;
            Globals.mSoundManager = mSoundManager;
            Globals.mRandom = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.mSpriteBatch = mSpriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            // setup texture manager
            mTextureManager.SetContentManager(Content);
            mTextureManager.SetSpriteBatch(mSpriteBatch);

            mSoundManager.LoadContent(Content, new List<string> { "quack" });

            // Load Star Textures
            mTextureManager.LoadTexture("sunTypeB", "GameObjects/Stars/sunTypeB");
            mTextureManager.LoadTexture("sunTypeF", "GameObjects/Stars/sunTypeF");
            mTextureManager.LoadTexture("sunTypeG", "GameObjects/Stars/sunTypeG");
            mTextureManager.LoadTexture("sunTypeK", "GameObjects/Stars/sunTypeK");
            mTextureManager.LoadTexture("sunTypeM", "GameObjects/Stars/sunTypeM");

            // Load Planet Textures
            mTextureManager.LoadTexture("planetTypeH", "GameObjects/Planets/planetTypeH");
            mTextureManager.LoadTexture("planetTypeJ", "GameObjects/Planets/planetTypeJ");
            mTextureManager.LoadTexture("planetTypeM", "GameObjects/Planets/planetTypeM");
            mTextureManager.LoadTexture("planetTypeY", "GameObjects/Planets/planetTypeY");

            // Load other STuff
            mTextureManager.LoadTexture("gameBackground", "gameBackground");
            mTextureManager.LoadTexture("Galaxy", "Galaxy");


            // game fonts
            mTextureManager.LoadSpriteTexture("text", "fonts/text");
            mTextureManager.LoadSpriteTexture("title", "fonts/title");
            mTextureManager.LoadSpriteTexture("smal", "fonts/smal");

            Globals.mLayerManager = mLayerManager = new LayerManager(this, GraphicsDevice, mSpriteBatch, Content, mSoundManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Exit();
            }
            // TODO: Add your update logic here
            // handle window resize
            if (mResulutionWasResized)
            {
                mLayerManager.OnResolutionChanged();
            }
            InputState inputState = mInputManager.Update();
            mLayerManager.Update(gameTime, inputState, Window, mGraphicsDeviceManager);
            System.Diagnostics.Debug.WriteLine(inputState.mMousePosition);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            mLayerManager.Draw();
            base.Draw(gameTime);
        }

        // Some Stuff
        public void ToggleFullscreen()
        {
            if (mIsFullScreen)
            {
                UnSetFullscreen();
            }
            else
            {
                SetFullscreen();
            }
            mIsFullScreen = !mIsFullScreen;
        }

        private void SetFullscreen()
        {
            mWidth = Window.ClientBounds.Width;
            mHeight = Window.ClientBounds.Height;

            mGraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            mGraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            mGraphicsDeviceManager.IsFullScreen = true;
            mGraphicsDeviceManager.ApplyChanges();
        }

        private void UnSetFullscreen()
        {
            mGraphicsDeviceManager.PreferredBackBufferWidth = mWidth;
            mGraphicsDeviceManager.PreferredBackBufferHeight = mHeight;
            mGraphicsDeviceManager.IsFullScreen = false;
            mGraphicsDeviceManager.ApplyChanges();
        }
    }
}