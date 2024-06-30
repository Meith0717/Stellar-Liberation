// UiElement.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public abstract class UiElement
    {
        private readonly Canvas Canvas;
        protected float mUiScale;
        protected Rectangle mRoot;

        public bool IsDisposed { get; protected set; }
        public UiElement() => Canvas = new Canvas();
        public virtual void Update(InputState inputState, GameTime gameTime) 
        {
            Canvas.UpdateFrame(mRoot, mUiScale);
        }

        public virtual void ApplyResolution(Rectangle root, Resolution resolution)
        {
            mUiScale = resolution.UiScaling;
            mRoot = root;
            Canvas.UpdateFrame(mRoot, mUiScale);
        }

        public abstract void Draw();
        public void DrawCanvas() => Canvas.Draw();

        // Position propeties
        public float RelX { set => Canvas.RelativeX = value; }
        public float RelY { set => Canvas.RelativeY = value; }
        public int X { set => Canvas.X = value; }
        public int Y { set => Canvas.Y = value; }

        // Dimension propeties
        public float RelWidth { set => Canvas.RelWidth = value; }
        public float RelHeight { set => Canvas.RelHeight = value; }
        public int Height { set => Canvas.Height = value; }
        public int Width { set => Canvas.Width = value; }

        // Optional propeties
        public int HSpace { set => Canvas.HSpace = value; }
        public int VSpace { set => Canvas.VSpace = value; }
        public Anchor Anchor { set => Canvas.Anchor = value; }
        public FillScale FillScale { set => Canvas.FillScale = value; }

        // Utilitys
        public bool Contains(Vector2 position) => Bounds.Contains(position);
        public Vector2 Offset => new(Bounds.Width / 2, Bounds.Height / 2);
        public Vector2 Center => Bounds.Center.ToVector2();
        public Vector2 Position => Bounds.Location.ToVector2();
        public Rectangle Bounds => Canvas.Bounds;
    }
}
