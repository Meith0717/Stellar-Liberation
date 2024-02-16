// UpgradableComponent.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement
{
    [Serializable]
    public abstract class UpgradableComponent
    {
        [JsonProperty] public int Level { get; private set; } = 1;

        protected UpgradableComponent(int level) => Level = level;

        public void IncrementLevel() => Level++;

        protected int GetLevelDependetValue(int value, float weightGreaterOne) => value * (int)Math.Pow(Level, 1 / weightGreaterOne);
    }
}
