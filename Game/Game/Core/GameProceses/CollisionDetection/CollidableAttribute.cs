// CollidableAttribute.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    public class CollidableAttribute : Attribute
    {
        public float Mass;

        public CollidableAttribute(float mass) => Mass = mass;

        public CollidableAttribute() => Mass = float.PositiveInfinity;
    }
}
