// UiFrame.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.Objects.UiElements
{
    public class UiFrame : UiElement
    {
        private Color mColor = new(0, 0, 0);
        public float Alpha = .9f;
        public bool mBorder;
        private readonly LinkedList<UiElement> mChildren = new();

        public UiFrame(bool border = true) => mBorder = border; 

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public void ClearChilds() => mChildren.Clear();

        public bool AnyChild() => mChildren.Any();

        #region Draw Frame with Roundetedges

        public override void Draw()
        {
            TextureManager.Instance.SpriteBatch.FillRectangle(Bounds, mColor * Alpha, 1);

            if (mBorder)
            {
                var boundsF = Bounds.ToRectangleF();

                var length = 100;
                var offset = 10;

                var topLeft = boundsF.TopLeft + new Vector2(offset);
                var topRight = boundsF.TopRight + new Vector2(-offset, offset);
                var bottomRight = boundsF.BottomRight - new Vector2(offset);
                var bottomLeft = boundsF.BottomLeft + new Vector2(offset, -offset);

                var color = Color.MonoGameOrange * Alpha;

                TextureManager.Instance.DrawLine(topLeft, topLeft + new Vector2(length, 0), color, 2, 0);
                TextureManager.Instance.DrawLine(topLeft, topLeft + new Vector2(0, length), color, 2, 0);

                TextureManager.Instance.DrawLine(topRight, topRight - new Vector2(length, 0), color, 2, 0);
                TextureManager.Instance.DrawLine(topRight, topRight + new Vector2(0, length), Color.MonoGameOrange, 2, 0);

                TextureManager.Instance.DrawLine(bottomRight, bottomRight - new Vector2(length, 0), color, 2, 0);
                TextureManager.Instance.DrawLine(bottomRight, bottomRight - new Vector2(0, length), color, 2, 0);

                TextureManager.Instance.DrawLine(bottomLeft, bottomLeft + new Vector2(length, 0), color, 2, 0);
                TextureManager.Instance.DrawLine(bottomLeft, bottomLeft - new Vector2(0, length), color, 2, 0);
            }

            foreach (var child in mChildren) child.Draw();
            Canvas.Draw();
        }
        #endregion

        public override void Update(InputState inputState, GameTime gameTime)
        {
            foreach (var child in mChildren)
                child.Update(inputState, gameTime);
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            Canvas.UpdateFrame(root, resolution.uiScaling);
            foreach (var child in mChildren)
                child.ApplyResolution(Bounds, resolution);
        }
    }
}

