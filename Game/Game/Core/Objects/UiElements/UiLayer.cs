// UiLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiLayer : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private LinkedList<UiElement> mChildren = new LinkedList<UiElement>();

        public override void Initialize(Rectangle root, float UiScaling)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.Initialize(mCanvas.Bounds, 1);
        }

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            TextureManager.Instance.SpriteBatch.FillRectangle(mCanvas.Bounds, color, 1);
            foreach (var child in mChildren) child.Draw();
            mCanvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root, float UiScaling)
        {
            foreach (var child in mChildren) child.Update(inputState, mCanvas.Bounds, 1);
        }

        public override void OnResolutionChanged(Rectangle root, float UiScaling)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.OnResolutionChanged(mCanvas.Bounds, 1);
        }
    }
}
