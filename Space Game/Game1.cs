using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Persistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace CelestialOdyssey
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Local Classes
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;

        // Global Classes
        private SpriteBatch mSpriteBatch;
        public readonly LayerManager mLayerManager;
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
            mGraphicsManager = new GraphicsDeviceManager(this);
            mLayerManager = new LayerManager(this);
            mSerialize = new Serialize();
            //ToggleFullscreen();
        }

        protected override void Initialize()
        {
            mGraphicsManager.ApplyChanges();
            base.Initialize();
            mLayerManager.AddLayer(new GameLayer(this));
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("cursor"), 0, 0));
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            System.Diagnostics.Debug.WriteLine(Path.GetFullPath("/Content"));

            // setup texture manager
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);

            SoundManager.Instance.LoadSoundEffects(Content, "hit", "SoundEffects/hit");
            SoundManager.Instance.LoadSoundEffects(Content, "collect", "SoundEffects/collect");
            SoundManager.Instance.CreateSoundEffectInstances();

            // Load Star Textures
            TextureManager.Instance.LoadTexture(Content, "StarLightAlpha", "GameObjects/Stars/StarLightAlpha");
            TextureManager.Instance.LoadTexture(Content, "M", "GameObjects/Stars/M");
            TextureManager.Instance.LoadTexture(Content, "K", "GameObjects/Stars/K");
            TextureManager.Instance.LoadTexture(Content, "G", "GameObjects/Stars/G");
            TextureManager.Instance.LoadTexture(Content, "F", "GameObjects/Stars/F");
            TextureManager.Instance.LoadTexture(Content, "A", "GameObjects/Stars/A");
            TextureManager.Instance.LoadTexture(Content, "B", "GameObjects/Stars/B");
            TextureManager.Instance.LoadTexture(Content, "O", "GameObjects/Stars/O");
            TextureManager.Instance.LoadTexture(Content, "BH", "GameObjects/Stars/BH");

            // Load Planet Textures
            string planetPath = "GameObjects/Planets/";
            for (int i = 1; i <= 4; i++) { TextureManager.Instance.LoadTexture(Content, $"cold{i}", $"{planetPath}cold{i}"); }
            for (int i = 1; i <= 6; i++) { TextureManager.Instance.LoadTexture(Content, $"dry{i}", $"{planetPath}dry{i}"); }
            for (int i = 1; i <= 4; i++) { TextureManager.Instance.LoadTexture(Content, $"gas{i}", $"{planetPath}gas{i}"); }
            for (int i = 1; i <= 6; i++) { TextureManager.Instance.LoadTexture(Content, $"stone{i}", $"{planetPath}stone{i}"); }
            for (int i = 1; i <= 8; i++) { TextureManager.Instance.LoadTexture(Content, $"terrestrial{i}", $"{planetPath}terrestrial{i}"); }
            for (int i = 1; i <= 4; i++) { TextureManager.Instance.LoadTexture(Content, $"warm{i}", $"{planetPath}warm{i}"); }
            TextureManager.Instance.LoadTexture(Content, "planetShadow", "GameObjects/Planets/planetShadow");

            // Load CrossHair
            TextureManager.Instance.LoadTexture(Content, "selectCrosshait", "GameObjects/crossHair/selectCrosshait");
            TextureManager.Instance.LoadTexture(Content, "targetCrosshair", "GameObjects/crossHair/targetCrosshair");

            // Load other STuff
            TextureManager.Instance.LoadTexture(Content, "gameBackground", "gameBackground");
            TextureManager.Instance.LoadTexture(Content, "gameBackgroundParlax1", "gameBackgroundParlax");
            TextureManager.Instance.LoadTexture(Content, "gameBackgroundParlax2", "gameBackgroundParlax2");
            TextureManager.Instance.LoadTexture(Content, "gameBackgroundParlax3", "gameBackgroundParlax3");
            TextureManager.Instance.LoadTexture(Content, "gameBackgroundParlax4", "gameBackgroundParlax4");
            TextureManager.Instance.LoadTexture(Content, "ship", "GameObjects/Ships/ship");
            TextureManager.Instance.LoadTexture(Content, "shipSekect", "GameObjects/Ships/shipSekect");
            TextureManager.Instance.LoadTexture(Content, "warpAnimation", "GameObjects/Ships/warpAnimation");
            TextureManager.Instance.LoadTexture(Content, "systemState", "GameObjects/systemState");
            TextureManager.Instance.LoadTexture(Content, "cursor", "cursor");
            TextureManager.Instance.LoadTexture(Content, "transparent", "GameObjects/transparent");
            TextureManager.Instance.LoadTexture(Content, "projectile", "GameObjects/projectile");
            TextureManager.Instance.LoadTexture(Content, "spaceStation", "GameObjects/spaceStation");
            // Items
            TextureManager.Instance.LoadTexture(Content, "odyssyum", "GameObjects/Items/odyssyum");
            TextureManager.Instance.LoadTexture(Content, "postyum", "GameObjects/Items/postyum");
            // Ui
            TextureManager.Instance.LoadTexture(Content, "Layer", "UserInterface/Layer/layer");
            TextureManager.Instance.LoadTexture(Content, "Circle", "UserInterface/Layer/circle");
            TextureManager.Instance.LoadTexture(Content, "buttonExitgame", "UserInterface/PauseLayer/buttonExitgame");
            TextureManager.Instance.LoadTexture(Content, "buttonContinue", "UserInterface/PauseLayer/buttonContinue");
            TextureManager.Instance.LoadTexture(Content, "level", "UserInterface/HUDLayer/level");
            TextureManager.Instance.LoadTexture(Content, "menue", "UserInterface/HUDLayer/menue");
            TextureManager.Instance.LoadTexture(Content, "alloys", "UserInterface/HUDLayer/Alloys");
            TextureManager.Instance.LoadTexture(Content, "energy", "UserInterface/HUDLayer/Energy");
            TextureManager.Instance.LoadTexture(Content, "minerals", "UserInterface/HUDLayer/Minerals");
            TextureManager.Instance.LoadTexture(Content, "exit", "UserInterface/HUDLayer/exit");
            TextureManager.Instance.LoadTexture(Content, "info", "UserInterface/HUDLayer/info");
            TextureManager.Instance.LoadTexture(Content, "deSelect", "UserInterface/HUDLayer/deSelect");
            TextureManager.Instance.LoadTexture(Content, "target", "UserInterface/HUDLayer/target");
            TextureManager.Instance.LoadTexture(Content, "stop", "UserInterface/HUDLayer/stop");

            // game fonts
            TextureManager.Instance.LoadSpriteTexture(Content, "text", "fonts/text");
            TextureManager.Instance.LoadSpriteTexture(Content, "title", "fonts/title");
            TextureManager.Instance.LoadSpriteTexture(Content, "smal", "fonts/smal");
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