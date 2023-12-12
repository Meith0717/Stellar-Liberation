// UiCanvas.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public enum Anchor { N, NE, E, SE, S, SW, W, NW, Center, Top, Bottom, Left, Right, CenterH, CenterV, None }
    public enum FillScale { X, Y, Both, None }

    public class UiCanvas
    {
        // UiFrame
        private RectangleF mCanvas;

        // Needet properties
        public float RelativeX;
        public float RelativeY;

        public float RelWidth = 0.1f;
        public float RelHeight = 0.1f;
        public float? Height;
        public float? Width;

        // Optional properties
        public int? HSpace;
        public int? VSpace;
        public Anchor Anchor = Anchor.None;
        public FillScale FillScale = FillScale.None;

        public void UpdateFrame(RectangleF root, float uiScale)
        {
            var x = root.X + (root.Width * RelativeX);
            var y = root.Y + (root.Height * RelativeY);

            var width = Width * uiScale ?? (int)(root.Width * RelWidth);
            var height = Height * uiScale ?? (int)(root.Height * RelHeight);

            ManageFillScale(root, ref x, ref y, ref width, ref height);
            ManageAnchor(root, ref x, ref y, ref width, ref height);
            ManageSpacing(root, ref x, ref y, ref width, ref height);

            mCanvas = new(x, y, width, height);
        }

        private void ManageFillScale(RectangleF root, ref float x, ref float y, ref float width, ref float height)
        {
            var aspectRatio = (float)width / height;

            switch (FillScale)
            {
                case FillScale.X:
                    width = root.Width;
                    height = width / aspectRatio;
                    break;
                case FillScale.Y:
                    height = root.Height;
                    width = height * aspectRatio;
                    break;
                case FillScale.Both:
                    x = 0; y = 0;
                    height = root.Height;
                    width = root.Width;
                    break;
            }
        }

        private void ManageAnchor(RectangleF root, ref float x, ref float y, ref float width, ref float height)
        {
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
                case Anchor.Left:
                    x = root.Left;
                    break;
                case Anchor.Right:
                    x = root.Right - width;
                    break;
                case Anchor.Top:
                    y = root.Top;
                    break;
                case Anchor.Bottom:
                    y = root.Bottom - height;
                    break;
                case Anchor.CenterH:
                    y = root.Center.Y - (height / 2);
                    break;
                case Anchor.CenterV:
                    x = root.Center.X - (width / 2);
                    break;
            }
        }

        private void ManageSpacing(RectangleF root, ref float x, ref float y, ref float width, ref float height)
        {
            if (HSpace != null)
            {
                var spaceLeft = x - root.Left;
                var spaceRight = root.Right - (width + x);

                if (spaceLeft < HSpace && spaceRight < HSpace)
                {
                    x += (float)HSpace;
                    width -= (float)HSpace * 2;
                }
                else
                {
                    if (spaceLeft < HSpace) x = root.X + (float)HSpace;
                    if (spaceRight < HSpace) x -= (float)HSpace;
                }
            }

            if (VSpace != null)
            {
                var spaceTop = y - root.Top;
                var spaceBottom = root.Bottom - (height + y);

                if (spaceTop < VSpace && spaceBottom < VSpace)
                {
                    y += (float)VSpace;
                    height -= (float)VSpace * 2;
                }
                else
                {
                    if (spaceTop < VSpace) y = root.Y + (float)VSpace;
                    if (spaceBottom < VSpace) y -= (float)VSpace;
                }
            }
        }


        public bool Contains(Vector2 position) => mCanvas.Contains(position);

        public Vector2 Offset => new Vector2(mCanvas.Width, mCanvas.Height) / 2;

        public Vector2 Center => mCanvas.Center;

        public Vector2 Position => mCanvas.Position;

        public RectangleF Bounds => mCanvas;

        public void Draw()
        {
            // TextureManager.Instance.DrawRectangleF(mCanvas, Color.Green, 2, 1);
        }
    }
}
