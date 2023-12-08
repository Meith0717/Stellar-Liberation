// Resolution.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ResolutionManagement
{
    public readonly struct Resolution
    {
        public readonly int Width;
        public readonly int Height;
        public readonly float uiScaling;

        public Resolution(int width, int height, float uiScaling) 
        {
            Width = width;
            Height = height;
            this.uiScaling = uiScaling; 
        }

        public override string ToString() => $"{Width}x{Height}";
    }
}
