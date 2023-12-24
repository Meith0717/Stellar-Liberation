// UiLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiLayer : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private LinkedList<UiElement> mChildren = new LinkedList<UiElement>();

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public void ClearChilds() => mChildren.Clear();

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            TextureManager.Instance.SpriteBatch.FillRectangle(Canvas.Bounds, color, 1);
            foreach (var child in mChildren) child.Draw();
            Canvas.Draw();
        }

        public override void Update(InputState inputState, RectangleF root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            foreach (var child in mChildren) child.Update(inputState, Canvas.Bounds, uiScaling);
        }
    }
}
