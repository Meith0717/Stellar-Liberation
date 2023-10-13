// Inventory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core.ItemManagement
{
    [Serializable]
    public class Inventory
    {
        [JsonProperty] private readonly Dictionary<Type, List<Item>> mItems;
    }
}
