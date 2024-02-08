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

        // Video settings
        [JsonProperty] public string Resolution = "800x480";
        [JsonProperty] public bool LimitRefreshRate = true;
        [JsonProperty] public long RefreshRate = 60;
        [JsonProperty] public bool Vsync = false;

        // Graphics settings
        [JsonProperty] public float ParticlesMultiplier = 1f;
    }
}
