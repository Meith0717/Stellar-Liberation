// Behavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.GameProceses.AI
{
    public abstract class Behavior
    {
        public abstract double GetScore();
        public abstract void Execute();

        public abstract void Recet();
    }
}
