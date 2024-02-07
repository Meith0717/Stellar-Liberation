// GameSettings.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.Core.CoreProceses.Persistance
{
    [Serializable]
    public class GameSettings
    {
        // Sound Settings
        [JsonProperty] public float MasterVolume = 1f;
        [JsonProperty] public float SoundEffectsVolume = 1f;
        [JsonProperty] public float MusicVolume = 1f;

        // Visual settings
        [JsonProperty] public string Resolution = "800x480";
    }
}
