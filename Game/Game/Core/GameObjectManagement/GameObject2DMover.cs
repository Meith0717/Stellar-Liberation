// GameObject2DMover.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.RootFinding;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    public static class GameObject2DMover
    {
        public static void Move(GameTime gameTime, GameObject2D gameObject2D, Scene scene)
        {
            gameObject2D.RemoveFromSpatialHashing(scene);
            gameObject2D.Position += gameObject2D.MovingDirection * gameObject2D.Velocity * gameTime.ElapsedGameTime.Milliseconds;
            gameObject2D.AddToSpatialHashing(scene);
        }
    }
}
