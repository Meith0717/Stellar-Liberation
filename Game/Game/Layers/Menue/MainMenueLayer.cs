// MainMenueLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;

namespace StellarLiberation.Game.Layers.MenueLayers
{
    public class MainMenueLayer : Layer
    {
        private UiFrame mFrame;

        public MainMenueLayer(Game1 game1) : base(game1, false)
        {
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(MenueSpriteRegistries.menueBackground) { FillScale = FillScale.FillIn, Anchor = Anchor.Center });

            mFrame.AddChild(new UiText(FontRegistries.titleFont, "Stellar\nLiberation") { Anchor = Anchor.NW, HSpace = 50, VSpace = 50 });

            mFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "New Game") { VSpace = 20, HSpace = 20, RelY = .5f, OnClickAction = () => LayerManager.AddLayer(new GameLayerManager(Game1)) });

            mFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Continue")
            {
                VSpace = 20,
                HSpace = 20,
                RelY = .6f,
                OnClickAction = () =>
            {
                LayerManager.AddLayer(new LoadingLayer(Game1, false));
                PersistanceManager.LoadAsync(new GameLayerManager(Game1), PersistanceManager.GameSaveFilePath, (gameState) => { LayerManager.PopLayer(); LayerManager.AddLayer(gameState); }, (ex) => throw ex);
            }
            });

            mFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Settings") { VSpace = 20, HSpace = 20, RelY = .7f, OnClickAction = () => LayerManager.AddLayer(new SettingsLayer(Game1)) });

            mFrame.AddChild(new UiButton(MenueSpriteRegistries.copyright, "") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = null });

            mFrame.AddChild(new UiButton(MenueSpriteRegistries.button, "Exit Game") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => LayerManager.Exit() });
        }

        public override void Destroy()
        {
            SoundEffectManager.Instance.StopAllSounds();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void ApplyResolution() 
        {
            mFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, LayerManager.Exit);
            mFrame.Update(inputState, gameTime);
        }
    }
}
