// Settings.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using MonoGame.Extended.Content;
using System;
using Newtonsoft.Json;

namespace StellarLiberation.Game.Core.CoreProceses.Persistance
{
    [Serializable]
    public class GameSettings 
    {
        // Sound Settings
        [JsonProperty] public static float MasterVolume = 1f;
        [JsonProperty] public static float SoundEffectsVolume = 1f;
        [JsonProperty] public static float MusicVolume = 1f;

        // Visual settings
        [JsonProperty] public static Resolution? Resolution = null;
    }
}
