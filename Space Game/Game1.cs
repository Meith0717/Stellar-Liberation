using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Persistance;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
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

        private GameLayer mGameLayer;

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
        }

        protected override void Initialize()
        {
            base.Initialize();
            mLayerManager = new LayerManager(this, GraphicsDevice, mSerialize);
            mGameLayer = new GameLayer();
            mLayerManager.AddLayer(mGameLayer);
        }

        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            System.Diagnostics.Debug.WriteLine(Path.GetFullPath("/Content"));

            // setup texture manager
            TextureManager.Instance.SetSpriteBatch(mSpriteBatch);
            foreach (Registry registry in ContentRegistry.IterateThroughRegistries())
            {
                if (TextureManager.Instance.LoadTexture(Content, registry.Name, registry.FilePath)) continue;
                SoundManager.Instance.LoadSoundEffects(Content, registry.Name, registry.FilePath);
            }
            SoundManager.Instance.CreateSoundEffectInstances();
            SetCursor(ContentRegistry.cursor);

            // game fonts
            TextureManager.Instance.LoadSpriteTexture(Content, "text", "fonts/text");
            TextureManager.Instance.LoadSpriteTexture(Content, "title", "fonts/title");
            TextureManager.Instance.LoadSpriteTexture(Content, "smal", "fonts/smal");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) Exit();
            if (mResulutionWasResized) mLayerManager.OnResolutionChanged();

            InputState inputState = mInputManager.Update();
            inputState.DoAction(ActionType.ToggleFullscreen, ToggleFullscreen);
            mLayerManager.Update(gameTime, inputState);

            inputState.DoAction(ActionType.Save, Save);
            inputState.DoAction(ActionType.Load, Load);

            base.Update(gameTime);
        }

        private void Save() { mSerialize.SerializeObject(mGameLayer, "test"); }
        private void Load() { mGameLayer = (GameLayer)mSerialize.PopulateObject(mGameLayer, "test"); }

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

        public void SetCursor(Registry registry)
        {
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
        }
    }
}