// MusicRegistries.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class MusicRegistries : Registries
    {
        private readonly static string soundEffects = @"music\";
        public readonly static Registry SciFi8 = new(soundEffects, "Sci-Fi 8");
        public readonly static Registry SciFi7 = new(soundEffects, "Sci-Fi 7");
        public readonly static Registry SciFi6 = new(soundEffects, "Sci-Fi 6");
        public readonly static Registry SciFi5 = new(soundEffects, "Sci-Fi 5");
        public readonly static Registry SciFi4 = new(soundEffects, "Sci-Fi 4");
        public readonly static Registry SciFi3 = new(soundEffects, "Sci-Fi 3");
        public readonly static Registry SciFi2 = new(soundEffects, "Sci-Fi 2");
        public readonly static Registry SciFi1 = new(soundEffects, "Sci-Fi 1");
    }
}
