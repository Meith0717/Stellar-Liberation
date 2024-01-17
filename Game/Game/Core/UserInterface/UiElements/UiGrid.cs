// UiGrid.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface.UiElements
{

    public class UiGrid : UiElement
    {
        private class UiGridElement : UiElement
        {
            public UiElement UiElement;

            public override void Draw() { UiElement?.Draw(); Canvas.Draw(); }

            public override void Update(InputState inputState, Rectangle root, float uiScaling)
            {
                Canvas.UpdateFrame(root, uiScaling);
                UiElement?.Update(inputState, Bounds, uiScaling);
            }
        }

        private readonly Dictionary<Vector2, UiGridElement> mGrid = new();

        public UiGrid(int i, int j)
        {
            for (int y = 0; y < j; y++)
            {
                for (int x = 0; x < i; x++)
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

        public bool Set(int i, int j, UiElement uiElement)
        {
            if (!mGrid.TryGetValue(new(i, j), out var gridElement)) return false;
            gridElement.UiElement = uiElement;
            return true;
        }

        public override void Update(InputState inputState, Rectangle root, float uiScaling)
        {
            Canvas.UpdateFrame(root, uiScaling);
            foreach (var elem in mGrid.Values) elem.Update(inputState, Canvas.Bounds, uiScaling);
        }

        public override void Draw()
        {
            foreach (var elem in mGrid.Values) elem.Draw();
            Canvas.Draw();
        }

    }
}
