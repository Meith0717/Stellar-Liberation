// UiFrame.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.Objects.UiElements
{
    public class UiFrame : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private readonly int mBorder;
        private readonly LinkedList<UiElement> mChildren = new LinkedList<UiElement>();

        public UiFrame(int border) 
        {
            if (border % 2 != 0) throw new System.Exception("Odd values lead to artefacts");
            mBorder = border;
        }

        public override void Initialize(Rectangle root, float UiScaling)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.Initialize(mCanvas.Bounds, 1);
        }

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));

            var bounds = mCanvas.Bounds;
            bounds.Inflate(-mBorder / 2, -mBorder / 2);

            var edgePositions = bounds.ToRectangleF().GetCorners();
            TextureManager.Instance.Draw(TextureRegistries.edge0, edgePositions[0] - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, color);
            TextureManager.Instance.Draw(TextureRegistries.edge1, edgePositions[1] - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, color);
            TextureManager.Instance.Draw(TextureRegistries.edge2, edgePositions[2] - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, color);
            TextureManager.Instance.Draw(TextureRegistries.edge3, edgePositions[3] - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, color);

            TextureManager.Instance.DrawLine(edgePositions[0] + new Vector2(mBorder / 2f, 0), edgePositions[1] + new Vector2(-mBorder / 2f, 0), color, mBorder, 1);
            TextureManager.Instance.DrawLine(edgePositions[1] + new Vector2(0, mBorder / 2f), edgePositions[2] + new Vector2(0, -mBorder / 2f), color, mBorder, 1);
            TextureManager.Instance.DrawLine(edgePositions[2] + new Vector2(-mBorder / 2f, 0), edgePositions[3] + new Vector2(mBorder / 2f, 0), color, mBorder, 1);
            TextureManager.Instance.DrawLine(edgePositions[3] + new Vector2(0, -mBorder / 2f), edgePositions[0] + new Vector2(0, +mBorder / 2f), color, mBorder, 1);

            bounds.Inflate(-mBorder / 2f, -mBorder / 2f);
            TextureManager.Instance.SpriteBatch.FillRectangle(bounds, color, 1);

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
