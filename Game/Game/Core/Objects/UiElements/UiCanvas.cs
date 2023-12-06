// UiCanvas.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

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
            var xL = root.X + X ?? (int)(root.X + (root.Width * RelX));
            var yL = root.Y + Y ?? (int)(root.Y + (root.Height * RelY));

            var width = Width ?? (int)(root.Width * RelWidth);
            var height = Height ?? (int)(root.Height * RelHeight);

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
                    xL = 0; yL = 0;
                    height = root.Height;
                    width = root.Width;
                    break;
            }

            switch (Anchor)
            {
                case Anchor.NW:
                    xL = root.X; yL = root.Y;
                    break;
                case Anchor.N:
                    xL = root.Center.X - (width / 2); yL = root.Y;
                    break;
                case Anchor.NE:
                    xL = root.Right - width; yL = root.Y;
                    break;
                case Anchor.E:
                    xL = root.Right - width; yL = root.Center.Y - (height / 2);
                    break;
                case Anchor.SE:
                    xL = root.Right - width; yL = root.Bottom - height;
                    break;
                case Anchor.S:
                    xL = root.Center.X - (width / 2); yL = root.Bottom - height;
                    break;
                case Anchor.SW:
                    xL = root.X; yL = root.Bottom - height;
                    break;
                case Anchor.W:
                    xL = root.X; yL = root.Center.Y - (height / 2);
                    break;
                case Anchor.Center:
                    xL = root.Center.X - (width / 2); yL = root.Center.Y - (height / 2);
                    break;
            }

            if (HSpace is not null)
            {
                var spaceLeft = xL - root.Left;
                if (spaceLeft < HSpace) xL = root.X + (int)HSpace;

                var SpaceRight = root.Right - (width + xL);
                if (SpaceRight < HSpace) xL -= (int)HSpace;
            }

            if (VSpace is not null)
            {
                var SpaceTop = yL - root.Top;
                if (SpaceTop < VSpace) yL = root.Y + (int)VSpace;

                var SpaceBottom = root.Bottom - (height + yL);
                if (SpaceBottom < VSpace) yL -= (int)VSpace;
            }

            mCanvas = new Rectangle(xL, yL, width, height);
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
