﻿// Resolution.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.CoreProceses.ResolutionManagement
{
    public readonly struct Resolution
    {
        public readonly int Width;
        public readonly int Height;
        public readonly float UiScaling;
        public readonly Rectangle Bounds;

        public Resolution(int width, int height, float uiScaling)
        {
            Width = width;
            Height = height;
            UiScaling = uiScaling;
            Bounds = new(0, 0, Width, Height);
        }

        public override string ToString() => $"{Width}x{Height}";
    }
}
