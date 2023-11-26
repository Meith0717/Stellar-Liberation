// Collidabel.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System;

namespace StellarLiberation.Game.Core.Collision_Detection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public class CollidableAttribute : Attribute
    {
        public CollidableAttribute() { }
    }
}
