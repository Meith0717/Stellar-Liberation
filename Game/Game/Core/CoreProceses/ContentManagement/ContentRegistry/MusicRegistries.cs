// MusicRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class MusicRegistries : Registries
    {
        private readonly static string soundEffects = @"music\";
        public readonly static Registry bgMusicGame = new(soundEffects, "DeepSpace10(loop)");
        public readonly static Registry bgMusicMenue = new(soundEffects, "Sci-Fi 5");
    }
}
