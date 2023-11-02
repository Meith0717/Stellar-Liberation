// UiElement.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Game.Core.UserInterface
{

    public enum Anchor { N, NE, E, SE, S, SW, W, NW, Center, None }
    public enum FillScale { X, Y, None }

    internal abstract class UiElement
    {
        public Rectangle Frame { get; private set; }

        public float RelX;
        public float RelY;
        public float RelWidth = 0.1f;
        public float RelHeight = 0.1f;
        public int? Height;
        public int? Width;
        public int HSpace;
        public int VSpace;
        public Anchor Anchor = Anchor.None;
        public FillScale FillScale = FillScale.None;

        public UiElement() { }

        public void UpdateFrame(Rectangle root)
        {
            var width = Width ?? (int)(root.Width * RelWidth);
            var height = Height ?? (int)(root.Height * RelHeight);
            var x = (int)(root.X + (root.Width * RelX));
            var y = (int)(root.Y + (root.Height * RelY));

            var aspectRatio = (float)width / height;

            switch (FillScale)
            {
                case FillScale.X:
                    width = root.Width; height = (int)(width / aspectRatio); 
                    break;
                case FillScale.Y:
                    height = root.Height; width = (int)(height * aspectRatio);
                    break;
            }

            var SpaceBottom = root.Bottom - (height + y);
            if (SpaceBottom < VSpace)
            {
                height = (root.Bottom - root.Top) - VSpace * 2;
                width = (int)(height * aspectRatio);
            }

            var SpaceRight = root.Right - (width + x);
            if (SpaceRight < HSpace)
            {
                width = (root.Right - root.Left) - HSpace * 2;
                height = (int)(width / aspectRatio);
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
                    x = root.Right - HSpace - width; y = root.Y;
                    break;
                case Anchor.E:
                    x = root.Right - HSpace - width; y = root.Center.Y - (height / 2);
                    break;
                case Anchor.SE:
                    x = root.Right - HSpace - width; y = root.Bottom - VSpace - height;
                    break;
                case Anchor.S:
                    x = root.Center.X - (width / 2); y = root.Bottom - VSpace - height;
                    break;
                case Anchor.SW:
                    x = root.X; y = root.Bottom - VSpace - height;
                    break;
                case Anchor.W:
                    x = root.X; y = root.Center.Y - (height/2);
                    break;
                case Anchor.Center:
                    x = root.Center.X - (width / 2); y = root.Center.Y - (height / 2);
                    break;
            }

            var spaceLeft = x - root.Left;
            if (spaceLeft < HSpace) x = root.X + HSpace;

            var SpaceTop = y - root.Top;
            if (SpaceTop < VSpace) y = root.Y + VSpace;

            Frame = new Rectangle(x, y, width, height);
        }

        public virtual void Initialize(Rectangle root) => UpdateFrame(root);
        public virtual void OnResolutionChanged(Rectangle root) => UpdateFrame(root);
        public abstract void Update(InputState inputState, Rectangle root);
        public abstract void Draw();
    }
}
