﻿// Canvas.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public enum Anchor { N, NE, E, SE, S, SW, W, NW, Center, Top, Bottom, Left, Right, CenterH, CenterV, None }
    public enum FillScale { X, Y, Both, FillIn, Fit, None }

    public class Canvas
    {
        // UiFrame
        public Rectangle Bounds;

        // Needet properties
        public int? X;
        public int? Y;
        public float RelativeX;
        public float RelativeY;

        public float RelWidth = 0.1f;
        public float RelHeight = 0.1f;
        public int? Height;
        public int? Width;

        // Optional properties
        public int? HSpace;
        public int? VSpace;
        public Anchor Anchor = Anchor.None;
        public FillScale FillScale = FillScale.None;

        public void UpdateFrame(Rectangle root, float uiScale)
        {
            var x = X ?? root.X + (int)(root.Width * RelativeX);
            var y = Y ?? root.Y + (int)(root.Height * RelativeY);

            var width = (int)(Width * uiScale ?? root.Width * RelWidth);
            var height = (int)(Height * uiScale ?? root.Height * RelHeight);

            ManageFillScale(root, ref x, ref y, ref width, ref height);
            ManageAnchor(root, ref x, ref y, ref width, ref height);
            ManageSpacing(root, ref x, ref y, ref width, ref height, HSpace.HasValue ? HSpace * uiScale : null, VSpace.HasValue ? VSpace * uiScale : null);

            Bounds = new(x, y, width, height);
        }

        #region Apply Resolution and Position

        private void ManageFillScale(Rectangle root, ref int x, ref int y, ref int width, ref int height)
        {
            var rootAspectRatio = (float)root.Width / (float)root.Height;
            var aspectRatio = (float)width / (float)height;

            switch (FillScale)
            {
                case FillScale.X:
                    width = root.Width;
                    height = (int)(width / aspectRatio);
                    break;
                case FillScale.Y:
                    height = root.Height;
                    width = (int)(height * aspectRatio);
                    break;
                case FillScale.Both:
                    x = root.X; y = root.Y;
                    height = root.Height;
                    width = root.Width;
                    break;
                case FillScale.FillIn:
                    if (aspectRatio > rootAspectRatio)
                    {
                        height = root.Height;
                        width = (int)(height * aspectRatio);
                    }
                    if (aspectRatio < rootAspectRatio)
                    {
                        width = root.Width;
                        height = (int)(width / aspectRatio);
                    }
                    if (aspectRatio == rootAspectRatio)
                    {
                        x = root.X; y = root.Y;
                        height = root.Height;
                        width = root.Width;
                    }
                    break;
                case FillScale.Fit:
                    if (aspectRatio > rootAspectRatio)
                    {
                        width = root.Width;
                        height = (int)(width / aspectRatio);
                    }
                    if (aspectRatio < rootAspectRatio)
                    {
                        height = root.Height;
                        width = (int)(height * aspectRatio);

                    }
                    if (aspectRatio == rootAspectRatio)
                    {
                        x = root.X; y = root.Y;
                        height = root.Height;
                        width = root.Width;
                    }
                    break;
            }
        }

        private void ManageAnchor(Rectangle root, ref int x, ref int y, ref int width, ref int height)
        {
            x = Anchor switch
            {
                Anchor.NW => root.X,
                Anchor.N => root.Center.X - (width / 2),
                Anchor.NE => root.Right - width,
                Anchor.E => root.Right - width,
                Anchor.SE => root.Right - width,
                Anchor.S => root.Center.X - (width / 2),
                Anchor.SW => root.X,
                Anchor.W => root.X,
                Anchor.Center => root.Center.X - (width / 2),
                Anchor.Left => root.Left,
                Anchor.Right => root.Right - width,
                Anchor.CenterV => root.Center.X - (width / 2),
                Anchor.Top => root.Left + root.Width / 2 - width / 2,
                Anchor.Bottom => root.Left + root.Width / 2 - width / 2,
                Anchor.CenterH => x,
                Anchor.None => x,
                _ => throw new System.NotImplementedException()
            };
            y = Anchor switch
            {
                Anchor.NW => root.Y,
                Anchor.N => root.Y,
                Anchor.NE => root.Y,
                Anchor.E => root.Center.Y - (height / 2),
                Anchor.SE => root.Bottom - height,
                Anchor.S => root.Bottom - height,
                Anchor.SW => root.Bottom - height,
                Anchor.W => root.Center.Y - (height / 2),
                Anchor.Center => root.Center.Y - (height / 2),
                Anchor.Top => root.Top,
                Anchor.Bottom => root.Bottom - height,
                Anchor.CenterH => root.Center.Y - (height / 2),
                Anchor.Left => root.Y + root.Height / 2 - height / 2,
                Anchor.Right => root.Y + root.Height / 2 - height / 2,
                Anchor.CenterV => y,
                Anchor.None => y,
                _ => throw new System.NotImplementedException()
            };
        }

        private void ManageSpacing(Rectangle root, ref int x, ref int y, ref int width, ref int height, float? hSpace, float? vSpace)
        {
            if (hSpace != null)
            {
                var spaceLeft = x - root.Left;
                var spaceRight = root.Right - (width + x);

                if (spaceLeft < hSpace && spaceRight < hSpace)
                {
                    x += (int)hSpace;
                    width -= (int)hSpace * 2;
                }
                else
                {
                    if (spaceLeft < hSpace) x = root.X + (int)hSpace;
                    if (spaceRight < hSpace) x -= (int)hSpace;
                }
            }

            if (vSpace != null)
            {
                var spaceTop = y - root.Top;
                var spaceBottom = root.Bottom - (height + y);

                if (spaceTop < vSpace && spaceBottom < vSpace)
                {
                    y += (int)vSpace;
                    height -= (int)vSpace * 2;
                }
                else
                {
                    if (spaceTop < vSpace) y = root.Y + (int)vSpace;
                    if (spaceBottom < vSpace) y -= (int)vSpace;
                }
            }
        }

        private Vector2 mLastMousePosition;

        public void MouseDrag(InputState inputState)
        {
            Vector2 delta = inputState.mMousePosition - mLastMousePosition;
            Bounds.X += (int)delta.X;
            Bounds.Y += (int)delta.Y;
            mLastMousePosition = inputState.mMousePosition;
        }

        #endregion


        public void Draw(bool debug = true)
        {
            if (debug) return;
            TextureManager.Instance.DrawRectangleF(Bounds, Color.Green, 1, 1);
        }
    }
}
