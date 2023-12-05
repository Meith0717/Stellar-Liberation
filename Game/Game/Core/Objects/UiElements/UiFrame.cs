// UiFrame.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;
using MonoGame.Extended;

namespace StellarLiberation.Game.Core.Objects.UiElements
{
    public class UiFrame : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private readonly int mDiameter;
        private readonly LinkedList<UiElement> mChildren = new LinkedList<UiElement>();

        public UiFrame(int border) 
        { 
            mDiameter = 2 * border;
        }

        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.Initialize(mCanvas.Bounds);
        }

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            var layer = TextureManager.Instance.GetTexture(TextureRegistries.layer);
            Point edge, nextEdge;

            for (int i = 0; i < 4; i++)
            {
                edge = mCanvas.Bounds.GetCorners()[i];
                nextEdge = mCanvas.Bounds.GetCorners()[(i + 1) % 4];

                TextureManager.Instance.Draw(TextureRegistries.circle, edge.ToVector2() - new Vector2(mDiameter, mDiameter) / 2, mDiameter, mDiameter, Color);
                TextureManager.Instance.DrawLine(edge.ToVector2(), nextEdge.ToVector2(), Color, mDiameter, 1);
            }
            TextureManager.Instance.SpriteBatch.Draw(layer, mCanvas.Bounds, color);

            foreach (var child in mChildren) child.Draw();
            mCanvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            foreach (var child in mChildren) child.Update(inputState, mCanvas.Bounds);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.OnResolutionChanged(mCanvas.Bounds);
        }
    }
}
