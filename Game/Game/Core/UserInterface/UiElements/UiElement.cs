// UiElement.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System;

namespace StellarLiberation.Game.Core.UserInterface
{
    public abstract class UiElement
    {
        protected UiCanvas Canvas;

        public UiElement() => Canvas = new UiCanvas();
        public abstract void Update(InputState inputState, RectangleF root, float uiScaling);
        public abstract void Draw();
        public RectangleF Bounds => Canvas.Bounds;

        // Position propeties
        public float RelX { set => Canvas.RelativeX = value; }
        public float RelY { set => Canvas.RelativeY = value; }

        // Dimension propeties
        public float RelWidth { set => Canvas.RelWidth = value; }
        public float RelHeight { set => Canvas.RelHeight = value; }
        public float Height { set => Canvas.Height = value; }
        public float Width { set => Canvas.Width = value; }

        // Optional propeties
        public int HSpace { set => Canvas.HSpace = value; }
        public int VSpace { set => Canvas.VSpace = value; }
        public Anchor Anchor { set => Canvas.Anchor = value; }
        public FillScale FillScale { set => Canvas.FillScale = value; }
    }
}
