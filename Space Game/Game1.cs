using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Persistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        public LayerManager mLayerManager;
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
            mSerialize = new Serialize();
            //ToggleFullscreen();
        }

        protected override void Initialize()
        {
            mGraphicsManager.ApplyChanges();
            mLayerManager = new LayerManager(this, GraphicsDevice);
            base.Initialize();
            mLayerManager.AddLayer(new GameLayer(new()));
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("textures/cursor"), 0, 0));
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            System.Diagnostics.Debug.WriteLine(Path.GetFullPath("/Content"));

            // setup texture manager
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
            foreach (Registry registry in ContentRegistry.Textures)
            {
                TextureManager.Instance.LoadTexture(Content, registry.Name, registry.FilePath);
            }

            SoundManager.Instance.LoadSoundEffects(Content, "torpedoHit", "SoundEffects/torpedoHit");
            SoundManager.Instance.LoadSoundEffects(Content, "torpedoFire", "SoundEffects/torpedoFire");
            SoundManager.Instance.LoadSoundEffects(Content, "collect", "SoundEffects/collect");
            SoundManager.Instance.CreateSoundEffectInstances();
            
            // // Ui
            // TextureManager.Instance.LoadTexture(Content, "Layer", "UserInterface/Layer/layer");
            // TextureManager.Instance.LoadTexture(Content, "Circle", "UserInterface/Layer/circle");
            // TextureManager.Instance.LoadTexture(Content, "buttonExitgame", "UserInterface/PauseLayer/buttonExitgame");
            // TextureManager.Instance.LoadTexture(Content, "buttonContinue", "UserInterface/PauseLayer/buttonContinue");
            // TextureManager.Instance.LoadTexture(Content, "level", "UserInterface/HUDLayer/level");
            // TextureManager.Instance.LoadTexture(Content, "menue", "UserInterface/HUDLayer/menue");
            // TextureManager.Instance.LoadTexture(Content, "alloys", "UserInterface/HUDLayer/Alloys");
            // TextureManager.Instance.LoadTexture(Content, "energy", "UserInterface/HUDLayer/Energy");
            // TextureManager.Instance.LoadTexture(Content, "minerals", "UserInterface/HUDLayer/Minerals");
            // TextureManager.Instance.LoadTexture(Content, "exit", "UserInterface/HUDLayer/exit");
            // TextureManager.Instance.LoadTexture(Content, "info", "UserInterface/HUDLayer/info");
            // TextureManager.Instance.LoadTexture(Content, "deSelect", "UserInterface/HUDLayer/deSelect");
            // TextureManager.Instance.LoadTexture(Content, "target", "UserInterface/HUDLayer/target");
            // TextureManager.Instance.LoadTexture(Content, "stop", "UserInterface/HUDLayer/stop");
            
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