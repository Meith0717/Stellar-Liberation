// FontRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class FontRegistries
    {
        private readonly static string fonts = @"fonts\";
        public readonly static Registry bgMusicGame = new(fonts, "debug");
        public readonly static Registry bgMusicMenue = new(fonts, "button");
    }
}
