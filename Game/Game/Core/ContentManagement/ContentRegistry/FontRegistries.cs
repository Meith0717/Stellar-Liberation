// FontRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class FontRegistries : Registries
    {
        private readonly static string fonts = @"fonts\";
        public readonly static Registry debug = new(fonts, "debug");
        public readonly static Registry button = new(fonts, "button");
    }
}
