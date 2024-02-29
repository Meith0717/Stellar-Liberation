// ICollidable.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public interface ICollidable
    {
        float Mass { get; }

        void HasCollide(Vector2 position, GameLayer scene);
    }
}
