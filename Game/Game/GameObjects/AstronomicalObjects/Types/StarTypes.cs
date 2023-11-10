﻿// StarTypes.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Utilitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    public static class StarTypes
    {
        // Colors
        private static Color TypeM = new(255, 177, 110);
        private static Color TypeK = new(255, 218, 187);
        private static Color TypeG = new(255, 237, 222);
        private static Color TypeF = new(243, 242, 255);
        private static Color TypeA = new(210, 223, 255);
        private static Color TypeB = new(181, 205, 255);
        private static Color TypeO = new(97, 130, 253);

        // Generate a random star
        public static Star GenerateRandomStar(Vector2 position)
        {
            List<Type> starTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Star).IsAssignableFrom(type) && type != typeof(Star))
                .ToList();

            var selectedStarType = ExtendetRandom.GetRandomElement(starTypes);
            ConstructorInfo constructor = selectedStarType.GetConstructor(new Type[] { typeof(Vector2) });

            if (constructor != null)
            {
                return (Star)constructor.Invoke(new object[] { position });
            }

            return null;
        }

        // Define common star constructor parameters
        private static readonly float CommonScaleDwarf = 25f;
        private static readonly float CommonScaleMain = 29f;
        private static readonly float CommonScaleGiants = 33f;
        private static readonly float CommonScaleSuperGiants = 37f;

        // Define nested star classes
        public class Dwarf
        {
            public class M : Star { public M(Vector2 position) : base(position, TextureRegistries.starM, CommonScaleDwarf, TypeM) { } }
            public class K : Star { public K(Vector2 position) : base(position, TextureRegistries.starK, CommonScaleDwarf, TypeK) { } }
            public class A : Star { public A(Vector2 position) : base(position, TextureRegistries.starA, CommonScaleDwarf, TypeA) { } }
        }

        public class Main
        {
            public class G : Star { public G(Vector2 position) : base(position, TextureRegistries.starG, CommonScaleMain, TypeG) { } }
            public class F : Star { public F(Vector2 position) : base(position, TextureRegistries.starF, CommonScaleMain, TypeF) { } }
            public class A : Star { public A(Vector2 position) : base(position, TextureRegistries.starA, CommonScaleMain, TypeA) { } }
            public class B : Star { public B(Vector2 position) : base(position, TextureRegistries.starB, CommonScaleMain, TypeB) { } }
        }

        public class Giants
        {
            public class K : Star { public K(Vector2 position) : base(position, TextureRegistries.starK, CommonScaleGiants, TypeK) { } }
            public class G : Star { public G(Vector2 position) : base(position, TextureRegistries.starG, CommonScaleGiants, TypeG) { } }
            public class F : Star { public F(Vector2 position) : base(position, TextureRegistries.starF, CommonScaleGiants, TypeF) { } }
        }

        public class SuperGiants
        {
            public class M : Star { public M(Vector2 position) : base(position, TextureRegistries.starM, CommonScaleSuperGiants, TypeM) { } }
            public class G : Star { public G(Vector2 position) : base(position, TextureRegistries.starG, CommonScaleSuperGiants, TypeG) { } }
            public class A : Star { public A(Vector2 position) : base(position, TextureRegistries.starA, CommonScaleSuperGiants, TypeA) { } }
            public class O : Star { public O(Vector2 position) : base(position, TextureRegistries.starO, CommonScaleSuperGiants, TypeO) { } }
        }
    }

}