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
            mTextureManager.LoadTexture("star", "star");
            mTextureManager.LoadTexture("spaceship", "spaceship");
            mTextureManager.LoadTexture("nebula", "nebula");
            // game fonts
            mTextureManager.LoadSpriteTexture("hud", "fonts/hud");

            Globals.mLayerManager = mLayerManager = new LayerManager(this, GraphicsDevice, mSpriteBatch, Content, mSoundManager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here
            // handle window resize
            if (mResulutionWasResized)
            {
                mLayerManager.OnResolutionChanged();
            }
            InputState inputState = mInputManager.Update();
            mLayerManager.Update(gameTime, inputState, Window, mGraphicsDeviceManager);
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
            throw new System.NotImplementedException();
        }
    }
}