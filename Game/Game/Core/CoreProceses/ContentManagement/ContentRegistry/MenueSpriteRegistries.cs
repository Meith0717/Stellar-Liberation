// MenueSpriteRegistries.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    internal class MenueSpriteRegistries : Registries
    {
        private readonly static string bar = @"textures\menueSprites\bar";
        public readonly static Registry barHorizontalLeft = new(bar, "barHorizontalLeft");
        public readonly static Registry barHorizontalMid = new(bar, "barHorizontalMid");
        public readonly static Registry barHorizontalRight = new(bar, "barHorizontalRight");
        public readonly static Registry barHorizontalShadowLeft = new(bar, "barHorizontalShadowLeft");
        public readonly static Registry barHorizontalShadowMid = new(bar, "barHorizontalShadowMid");
        public readonly static Registry barHorizontalShadowRight = new(bar, "barHorizontalShadowRight");
        public readonly static Registry barVerticalBottom = new(bar, "barVerticalBottom");
        public readonly static Registry barVerticalMid = new(bar, "barVerticalMid");
        public readonly static Registry barVerticalTop = new(bar, "barVerticalTop");
        public readonly static Registry barVerticalShadowBottom = new(bar, "barVerticalShadowBottom");
        public readonly static Registry barVerticalShadowMid = new(bar, "barVerticalShadowMid");
        public readonly static Registry barVerticalShadowTop = new(bar, "barVerticalShadowTop");

        private readonly static string buttons = @"textures\menueSprites\buttons";
        public readonly static Registry arrowL = new(buttons, "arrowL");
        public readonly static Registry arrowR = new(buttons, "arrowR");
        public readonly static Registry button = new(buttons, "button1");
        public readonly static Registry copyright = new(buttons, "copyright");
        public readonly static Registry dot = new(buttons, "dot");
        public readonly static Registry menu = new(buttons, "menu");
        public readonly static Registry pause = new(buttons, "pause");
        public readonly static Registry map = new(buttons, "map");
        public readonly static Registry xMark = new(buttons, "xMark");
        public readonly static Registry toggleOn = new(buttons, "toggleOn");
        public readonly static Registry toggleOff = new(buttons, "toggleOff");

        private readonly static string layer = @"textures\menueSprites\layer";
        public readonly static Registry edge0 = new(layer, "edge0");
        public readonly static Registry edge1 = new(layer, "edge1");
        public readonly static Registry edge2 = new(layer, "edge2");
        public readonly static Registry edge3 = new(layer, "edge3");

        private readonly static string textures = @"textures\menueSprites\textures";
        public readonly static Registry cursor = new(textures, "cursor");
        public readonly static Registry loading = new(textures, "loading");
        public readonly static Registry menueBackground = new(textures, "menueBackground");
        public readonly static Registry propulsion = new(textures, "propulsion");
        public readonly static Registry shield = new(textures, "shield");
        public readonly static Registry ship = new(textures, "ship");
        public readonly static Registry planet = new(textures, "planet");
        public readonly static Registry temperature = new(textures, "temperature");
    }
}
