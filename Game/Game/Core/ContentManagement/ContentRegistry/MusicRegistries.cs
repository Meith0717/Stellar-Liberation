// MusicRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class MusicRegistries
    {
        private readonly static string soundEffects = @"music\";
        public readonly static Registry bgMusicGame = new(soundEffects, "bgMusicGame");
        public readonly static Registry bgMusicMenue = new(soundEffects, "bgMusicMenue");
    }
}
