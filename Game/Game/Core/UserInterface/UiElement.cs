// UiElement.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.InputManagement;

namespace StellarLiberation.Game.Core.UserInterface
{
    public abstract class UiElement
    {
        protected UiCanvas mCanvas;

        public UiElement() => mCanvas = new UiCanvas();

        public abstract void Initialize(Rectangle root);
        public abstract void Update(InputState inputState, Rectangle root);
        public abstract void OnResolutionChanged(Rectangle root);
        public abstract void Draw();

        // Position propeties
        public float RelX { set => mCanvas.RelX = value; }
        public float RelY { set => mCanvas.RelY = value; }
        public int X { set => mCanvas.X = value; }
        public int Y { set => mCanvas.Y = value; }

        // Dimension propeties
        public float RelWidth { set => mCanvas.RelWidth = value; }
        public float RelHeight { set => mCanvas.RelHeight = value; }
        public int Height { set => mCanvas.Height = value; }
        public int Width { set => mCanvas.Width = value; }

        // Optional propeties
        public int HSpace { set => mCanvas.HSpace = value; }
        public int VSpace { set => mCanvas.VSpace = value; }
        public Anchor Anchor { set => mCanvas.Anchor = value; }
        public FillScale FillScale { set => mCanvas.FillScale = value; }


    }
}
