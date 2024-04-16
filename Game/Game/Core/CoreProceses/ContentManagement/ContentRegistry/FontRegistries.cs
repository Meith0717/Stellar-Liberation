// FontRegistries.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Graphics;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class FontRegistries : Registries
    {
        private readonly static string fonts = @"fonts\";
        public static Registry debug = new(fonts, "debug");
        public readonly static Registry text = new(fonts, "text");
        public readonly static Registry title = new(fonts, "title");
    }
}
