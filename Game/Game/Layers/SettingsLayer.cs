// SettingsLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.Persistance;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Core.GameEngine.Content_Management;

namespace StellarLiberation.Game.Layers
{
    internal class SettingsLayer : Layer
    {
        private UiLayer mFrame;
        private UiSlider mSlider;

        public SettingsLayer(bool updateBelow) : base(updateBelow)
        {
            mFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mFrame.AddChild(new UiSprite(TextureRegistries.menueBackground) { FillScale = FillScale.X });

            mFrame.AddChild(new UiButton(TextureRegistries.button, "< Back") { VSpace = 20, HSpace = 20, Anchor = Anchor.SW, OnClickAction = () => mLayerManager.PopLayer() });
            mFrame.AddChild(new UiButton(TextureRegistries.button, "Apply") { VSpace = 20, HSpace = 20, Anchor = Anchor.SE, OnClickAction = ApplyChanges });


            mSlider = new(MusicManager.Instance.OverallVolume) { Width = 500, RelX = .5f, RelY = .5f};
            mFrame.AddChild(mSlider);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mFrame.Initialize(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mFrame.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Bounds);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrame.Update(inputState, mGraphicsDevice.Viewport.Bounds);
            MusicManager.Instance.ChangeOverallVolume(mSlider.Value);
        }

        private void ApplyChanges()
        {
        }
    }
}
