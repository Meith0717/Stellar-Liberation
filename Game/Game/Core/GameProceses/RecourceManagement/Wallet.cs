// Wallet.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    [Serializable]
    public class Wallet
    {
        [JsonProperty] 
        public int Balance { get; private set; }

        public void Add(int amount) => Balance += amount;

        public bool Remove(int amount)
        {
            if (Balance < amount) return false;
            Balance -= amount;
            return true;
        }
    }
}
