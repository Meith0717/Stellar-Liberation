﻿// UiGrid.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{

    public class UiGrid : UiElement
    {
        private class UiGridElement : UiElement
        {
            public UiElement UiElement;

            public override void Draw() { UiElement?.Draw(); DrawCanvas(); }

            public override void ApplyResolution(Rectangle root, Resolution resolution)
            {
                base.ApplyResolution(root, resolution);
                UiElement?.ApplyResolution(Bounds, resolution);
            }

            public override void Update(InputState inputState, GameTime gameTime) => UiElement?.Update(inputState, gameTime);
        }

        private readonly Dictionary<Vector2, UiGridElement> mGrid = new();

        public UiGrid(int i, int j)
        {
            for (float y = 0; y < j; y++)
            {
                for (float x = 0; x < i; x++)
                {
                    var relX = 1f / i * x;
                    var relY = 1f / j * y;
                    var width = 1f / i;
                    var height = 1f / j;

                    var grid = new UiGridElement { RelX = relX, RelY = relY, RelWidth = width, RelHeight = height };
                    mGrid.Add(new(x, y), grid);
                }
            }
        }

        public UiGrid(List<double> row, List<double> columns)
        {
            float cumulativeX = 0;
            int x = 0;
            foreach (var r in row)
            {
                int y = 0;
                float cumulativeY = 0;
                foreach (var c in columns)
                {
                    var relX = cumulativeX;
                    var relY = cumulativeY;
                    var width = (float)r;
                    var height = (float)c;
                    var grid = new UiGridElement { RelX = relX, RelY = relY, RelWidth = width, RelHeight = height };
                    mGrid.Add(new(x, y), grid);
                    cumulativeY += (float)c;
                    y++;
                }
                cumulativeX += (float)r;
                x++;
            }
        }

        public bool Set(int i, int j, UiElement uiElement)
        {
            if (!mGrid.TryGetValue(new(i, j), out var gridElement)) return false;
            gridElement.UiElement = uiElement;
            return true;
        }

        public override void Update(InputState inputState, GameTime gameTime)
        {
            foreach (var child in mGrid.Values)
                child.Update(inputState, gameTime);
        }

        public override void Draw()
        {
            foreach (var elem in mGrid.Values) elem.Draw();
            DrawCanvas();
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            base.ApplyResolution(root, resolution);
            foreach (var child in mGrid.Values)
                child.ApplyResolution(Bounds, resolution);
        }
    }
}
