// Vector2Extension.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.Extensions
{
    internal static class Vector2Extension
    {
        public static Vector2 DirectionToVector2(this Vector2 vector, Vector2 target) =>  Vector2.Normalize(Vector2.Subtract(target, vector));
    }
}
