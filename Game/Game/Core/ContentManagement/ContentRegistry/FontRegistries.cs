// FontRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class FontRegistries : Registries
    {
        private readonly static string fonts = @"fonts\";
        public readonly static Registry debugFont = new(fonts, "debugFont");
        public readonly static Registry buttonFont = new(fonts, "buttonFont");
        public readonly static Registry textFont = new(fonts, "textFont");
        public readonly static Registry titleFont = new(fonts, "titleFont");
    }
}
