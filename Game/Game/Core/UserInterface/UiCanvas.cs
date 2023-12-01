// UiElement.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    public enum Anchor { N, NE, E, SE, S, SW, W, NW, Center, None }
    public enum FillScale { X, Y, Both, None }

    public class UiCanvas
    {
        // UiFrame
        private Rectangle mCanvas;

        // Needet properties
        public float RelX;
        public float RelY;
        public int? X;
        public int? Y;

        public float RelWidth = 0.1f;
        public float RelHeight = 0.1f;
        public int? Height;
        public int? Width;

        // Optional properties
        public int? HSpace;
        public int? VSpace;
        public Anchor Anchor = Anchor.None;
        public FillScale FillScale = FillScale.None;

        public void UpdateFrame(Rectangle root)
        {
            var width = Width ??
                (int)(root.Width * RelWidth);
            var height = Height ??
                (int)(root.Height * RelHeight);
            var x = root.X + X ??
                (int)(root.X + (root.Width * RelX));
            var y = root.Y + Y ??
                (int)(root.Y + (root.Height * RelY));

            var aspectRatio = (float)width / height;

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
                    x = 0; y = 0;
                    height = root.Height;
                    width = root.Width;
                    break;
            }

            switch (Anchor)
            {
                case Anchor.NW:
                    x = root.X; y = root.Y;
                    break;
                case Anchor.N:
                    x = root.Center.X - (width / 2); y = root.Y;
                    break;
                case Anchor.NE:
                    x = root.Right - width; y = root.Y;
                    break;
                case Anchor.E:
                    x = root.Right - width; y = root.Center.Y - (height / 2);
                    break;
                case Anchor.SE:
                    x = root.Right - width; y = root.Bottom - height;
                    break;
                case Anchor.S:
                    x = root.Center.X - (width / 2); y = root.Bottom - height;
                    break;
                case Anchor.SW:
                    x = root.X; y = root.Bottom - height;
                    break;
                case Anchor.W:
                    x = root.X; y = root.Center.Y - (height / 2);
                    break;
                case Anchor.Center:
                    x = root.Center.X - (width / 2); y = root.Center.Y - (height / 2);
                    break;
            }

            if (HSpace is not null)
            {
                var spaceLeft = x - root.Left;
                if (spaceLeft < HSpace) x = root.X + (int)HSpace;

                var SpaceRight = root.Right - (width + x);
                if (SpaceRight < HSpace)
                {
                    width -= (int)MathF.Abs(SpaceRight - (int)HSpace);
                    height = (int)(width / aspectRatio);
                }
            }

            if (VSpace is not null)
            {
                var SpaceTop = y - root.Top;
                if (SpaceTop < VSpace) y = root.Y + (int)VSpace;

                var SpaceBottom = root.Bottom - (height + y);
                if (SpaceBottom < VSpace)
                {
                    height -= (int)MathF.Abs(SpaceBottom - (int)VSpace);
                    width = (int)(height * aspectRatio);
                }
            }

            mCanvas = new Rectangle(x, y, width, height);
        }

        public bool Contains(Vector2 position) => mCanvas.Contains(position);

        public Vector2 Offset => new Vector2(mCanvas.Width, mCanvas.Height) / 2;

        public Vector2 Center => mCanvas.Center.ToVector2();

        public Vector2 Position => mCanvas.Location.ToVector2();

        public Rectangle Bounds => mCanvas;

        public void Draw()
        {
            //TextureManager.Instance.DrawRectangleF(mCanvas, Color.Green, 2, 1);
        }
    }
}
