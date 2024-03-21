// UiFrame.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.Objects.UiElements
{
    public class UiFrame : UiElement
    {
        public Color Color = new(30, 34, 34);
        public double Alpha = .95f;
        private float mBorder;
        private readonly LinkedList<UiElement> mChildren = new();

        public UiFrame(int border = 16)
        {
            if (border % 2 != 0) throw new System.Exception("Odd values lead to artefacts");
            mBorder = border;
        }

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public void ClearChilds() => mChildren.Clear();

        public bool AnyChild() => mChildren.Any();

        #region Draw Frame with Roundetedges

        public override void Draw()
        {
            var innerColor = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));

            if (mBorder == 0)
            {
                TextureManager.Instance.SpriteBatch.FillRectangle(Bounds, innerColor, 1);
            }
            else
            {
                var bounds = Canvas.Bounds;
                bounds.Inflate(-mBorder / 2f, -mBorder / 2f);

                var edgePositions = bounds.GetCorners();
                TextureManager.Instance.Draw(MenueSpriteRegistries.edge0, edgePositions[0].ToVector2() - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, innerColor);
                TextureManager.Instance.Draw(MenueSpriteRegistries.edge1, edgePositions[1].ToVector2() - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, innerColor);
                TextureManager.Instance.Draw(MenueSpriteRegistries.edge2, edgePositions[2].ToVector2() - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, innerColor);
                TextureManager.Instance.Draw(MenueSpriteRegistries.edge3, edgePositions[3].ToVector2() - new Vector2(mBorder, mBorder) / 2f, mBorder, mBorder, innerColor);

                TextureManager.Instance.DrawLine(edgePositions[0].ToVector2() + new Vector2(mBorder / 2f, 0), edgePositions[1].ToVector2() + new Vector2(-mBorder / 2f, 0), innerColor, mBorder, 1);
                TextureManager.Instance.DrawLine(edgePositions[1].ToVector2() + new Vector2(0, mBorder / 2f), edgePositions[2].ToVector2() + new Vector2(0, -mBorder / 2f), innerColor, mBorder, 1);
                TextureManager.Instance.DrawLine(edgePositions[2].ToVector2() + new Vector2(-mBorder / 2f, 0), edgePositions[3].ToVector2() + new Vector2(mBorder / 2f, 0), innerColor, mBorder, 1);
                TextureManager.Instance.DrawLine(edgePositions[3].ToVector2() + new Vector2(0, -mBorder / 2f), edgePositions[0].ToVector2() + new Vector2(0, +mBorder / 2f), innerColor, mBorder, 1);

                bounds.Inflate(-mBorder / 2f, -mBorder / 2f);
                TextureManager.Instance.SpriteBatch.FillRectangle(bounds, innerColor, 1);
            }

            foreach (var child in mChildren) child.Draw();
            Canvas.Draw();
        }
        #endregion

        private bool mLastDragState;
        public override void Update(InputState inputState, GameTime gameTime)
        {
            foreach (var child in mChildren)
                child.Update(inputState, gameTime);
            // if (!Bounds.Contains(inputState.mMousePosition) && !mLastDragState) return;
            // mLastDragState = false;
            // if (!inputState.HasAction(ActionType.LeftClickHold)) return;
            // Canvas.MouseDrag(inputState);
            // mLastDragState = true;
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            Canvas.UpdateFrame(root, resolution.uiScaling);
            foreach (var child in mChildren)
                child.ApplyResolution(Bounds, resolution);
        }
    }
}
