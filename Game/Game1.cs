// Game1.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Layers;

namespace StellarLiberation
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Local Classes
        private readonly GraphicsDeviceManager mGraphicsManager;
        private readonly InputManager mInputManager;
        private readonly ResolutionManager ResolutionManager;
        private readonly Serialize Serialize;
        private SpriteBatch mSpriteBatch;
        private LayerManager mLayerManager;

        public Game1()
        {
            Content.RootDirectory = "Content";
            mGraphicsManager = new(this);
            mInputManager = new();
            Serialize = new();
            ResolutionManager = new(mGraphicsManager);

            // Window properties
            IsMouseVisible = true;
            Window.AllowAltF4 = false;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            mLayerManager = new(this, GraphicsDevice, Serialize, ResolutionManager);
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
            InputState inputState = mInputManager.Update();
            inputState.DoAction(ActionType.ToggleFullscreen, ResolutionManager.ToggleFullscreen);
            inputState.DoAction(ActionType.IncreaseScaling, () => ResolutionManager.UiScaling += 0.01f);
            inputState.DoAction(ActionType.DecreaseScaling, () => ResolutionManager.UiScaling -= 0.01f);
            mLayerManager.Update(gameTime, inputState);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            mLayerManager.Draw(mSpriteBatch);
            base.Draw(gameTime);
        }

        public void SetCursorTexture(Registry registry) => Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>(registry.FilePath), 0, 0));
    }
}