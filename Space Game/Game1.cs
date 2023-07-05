using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.GameEngine.Persistance;
using GalaxyExplovive.Core.GameEngine.Rendering;
using GalaxyExplovive.Core.LayerManagement;
using GalaxyExplovive.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GalaxyExplovive
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Local Classes
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;

        // Global Classes
        private SpriteBatch mSpriteBatch;
        public readonly LayerManager mLayerManager;
        public readonly TextureManager mTextureManager;
        public readonly SoundManager mSoundManager;
        public readonly Serialize mSerialize;

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

            mInputManager = new InputManager();
            mSoundManager = new SoundManager();
            mTextureManager = new TextureManager();
            mGraphicsManager = new GraphicsDeviceManager(this);
            mLayerManager = new LayerManager(this);
            mSerialize = new Serialize();
            //ToggleFullscreen();
        }

        protected override void Initialize()
        {
            mGraphicsManager.ApplyChanges();
            base.Initialize();
            mLayerManager.AddLayer(new TestLayer(this));
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 0, 0));
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // setup texture manager
            mTextureManager.SetSpriteBatch(mSpriteBatch);

            mSoundManager.LoadSoundEffects(Content, "hit", "SoundEffects/hit");
            mSoundManager.CreateSoundEffectInstances();

            // Load Star Textures
            mTextureManager.LoadTexture(Content, "StarLightAlpha", "GameObjects/Stars/StarLightAlpha");
            mTextureManager.LoadTexture(Content, "M", "GameObjects/Stars/M");
            mTextureManager.LoadTexture(Content, "K", "GameObjects/Stars/K");
            mTextureManager.LoadTexture(Content, "G", "GameObjects/Stars/G");
            mTextureManager.LoadTexture(Content, "F", "GameObjects/Stars/F");
            mTextureManager.LoadTexture(Content, "A", "GameObjects/Stars/A");
            mTextureManager.LoadTexture(Content, "B", "GameObjects/Stars/B");
            mTextureManager.LoadTexture(Content, "O", "GameObjects/Stars/O");
            mTextureManager.LoadTexture(Content, "BH", "GameObjects/Stars/BH");

            // Load Planet Textures
            string planetPath = "GameObjects/Planets/";
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture(Content, $"cold{i}", $"{planetPath}cold{i}"); }
            for (int i = 1; i <= 6; i++) { mTextureManager.LoadTexture(Content, $"dry{i}", $"{planetPath}dry{i}"); }
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture(Content, $"gas{i}", $"{planetPath}gas{i}"); }
            for (int i = 1; i <= 6; i++) { mTextureManager.LoadTexture(Content, $"stone{i}", $"{planetPath}stone{i}"); }
            for (int i = 1; i <= 8; i++) { mTextureManager.LoadTexture(Content, $"terrestrial{i}", $"{planetPath}terrestrial{i}"); }
            for (int i = 1; i <= 4; i++) { mTextureManager.LoadTexture(Content, $"warm{i}", $"{planetPath}warm{i}"); }
            mTextureManager.LoadTexture(Content, "planetShadow", "GameObjects/Planets/planetShadow");

            // Load CrossHair
            mTextureManager.LoadTexture(Content, "selectCrosshait", "GameObjects/crossHair/selectCrosshait");
            mTextureManager.LoadTexture(Content, "targetCrosshair", "GameObjects/crossHair/targetCrosshair");

            // Load other STuff

            mTextureManager.LoadTexture(Content, "gameBackground", "gameBackground");
            mTextureManager.LoadTexture(Content, "gameBackgroundParlax1", "gameBackgroundParlax");
            mTextureManager.LoadTexture(Content, "gameBackgroundParlax2", "gameBackgroundParlax2");
            mTextureManager.LoadTexture(Content, "gameBackgroundParlax3", "gameBackgroundParlax3");
            mTextureManager.LoadTexture(Content, "gameBackgroundParlax4", "gameBackgroundParlax4");
            mTextureManager.LoadTexture(Content, "ship", "GameObjects/Ships/ship");
            mTextureManager.LoadTexture(Content, "shipSekect", "GameObjects/Ships/shipSekect");
            mTextureManager.LoadTexture(Content, "warpAnimation", "GameObjects/Ships/warpAnimation");
            mTextureManager.LoadTexture(Content, "systemState", "GameObjects/systemState");
            mTextureManager.LoadTexture(Content, "cursor", "cursor");
            mTextureManager.LoadTexture(Content, "transparent", "GameObjects/transparent");
            mTextureManager.LoadTexture(Content, "projectile", "GameObjects/projectile");
            mTextureManager.LoadTexture(Content, "spaceStation", "GameObjects/spaceStation");

            // Ui
            mTextureManager.LoadTexture(Content, "Layer", "UserInterface/Layer/layer");
            mTextureManager.LoadTexture(Content, "Circle", "UserInterface/Layer/circle");
            mTextureManager.LoadTexture(Content, "buttonExitgame", "UserInterface/PauseLayer/buttonExitgame");
            mTextureManager.LoadTexture(Content, "buttonContinue", "UserInterface/PauseLayer/buttonContinue");
            mTextureManager.LoadTexture(Content, "level", "UserInterface/HUDLayer/level");
            mTextureManager.LoadTexture(Content, "menue", "UserInterface/HUDLayer/menue");
            mTextureManager.LoadTexture(Content, "alloys", "UserInterface/HUDLayer/Alloys");
            mTextureManager.LoadTexture(Content, "energy", "UserInterface/HUDLayer/Energy");
            mTextureManager.LoadTexture(Content, "minerals", "UserInterface/HUDLayer/Minerals");
            mTextureManager.LoadTexture(Content, "exit", "UserInterface/HUDLayer/exit");
            mTextureManager.LoadTexture(Content, "info", "UserInterface/HUDLayer/info");
            mTextureManager.LoadTexture(Content, "deSelect", "UserInterface/HUDLayer/deSelect");
            mTextureManager.LoadTexture(Content, "target", "UserInterface/HUDLayer/target");
            mTextureManager.LoadTexture(Content, "stop", "UserInterface/HUDLayer/stop");

            // game fonts
            mTextureManager.LoadSpriteTexture(Content, "text", "fonts/text");
            mTextureManager.LoadSpriteTexture(Content, "title", "fonts/title");
            mTextureManager.LoadSpriteTexture(Content, "smal", "fonts/smal");
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
            InputState inputState = mInputManager.Update();
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
            mGraphicsManager.ApplyChanges();
        }

        private void SetFullscreen()
        {
            mWidth = Window.ClientBounds.Width;
            mHeight = Window.ClientBounds.Height;

            mGraphicsManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            mGraphicsManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            mGraphicsManager.IsFullScreen = true;
        }

        private void UnSetFullscreen()
        {
            mGraphicsManager.PreferredBackBufferWidth = mWidth;
            mGraphicsManager.PreferredBackBufferHeight = mHeight;
            mGraphicsManager.IsFullScreen = false;
        }
    }
}