using CelestialOdyssey.GameEngine.Content_Management;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Linq;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types
{
    public static class StarTypes
    {
        // Colors
        public static Color TypeM = new Color(255, 177, 110);
        public static Color TypeK = new Color(255, 218, 187);
        public static Color TypeG = new Color(255, 237, 222);
        public static Color TypeF = new Color(243, 242, 255);
        public static Color TypeA = new Color(210, 223, 255);
        public static Color TypeB = new Color(181, 205, 255);
        public static Color TypeO = new Color(97, 130, 253);

        // Generate a random star
        public static Star GenerateRandomStar(Vector2 position)
        {
            List<Type> starTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Star).IsAssignableFrom(type) && type != typeof(Star))
                .ToList();

            int randomIndex = new Random().Next(0, starTypes.Count);
            Type selectedStarType = starTypes[randomIndex];
            ConstructorInfo constructor = selectedStarType.GetConstructor(new Type[] { typeof(Vector2) });

            if (constructor != null)
            {
                return (Star)constructor.Invoke(new object[] { position });
            }

            return null;
        }

        // Define common star constructor parameters
        private static readonly float CommonScaleDwarf = 10f;
        private static readonly float CommonScaleMain = 15f;
        private static readonly float CommonScaleGiants = 20f;
        private static readonly float CommonScaleSuperGiants = 25f;

        // Define nested star classes
        public class Dwarf
        {
            public class M : Star { public M(Vector2 position) : base(position, ContentRegistry.starM, CommonScaleDwarf, TypeM) { } }
            public class K : Star { public K(Vector2 position) : base(position, ContentRegistry.starK, CommonScaleDwarf, TypeK) { } }
            public class A : Star { public A(Vector2 position) : base(position, ContentRegistry.starA, CommonScaleDwarf, TypeA) { } }
        }

        public class Main
        {
            public class G : Star { public G(Vector2 position) : base(position, ContentRegistry.starG, CommonScaleMain, TypeG) { } }
            public class F : Star { public F(Vector2 position) : base(position, ContentRegistry.starF, CommonScaleMain, TypeF) { } }
            public class A : Star { public A(Vector2 position) : base(position, ContentRegistry.starA, CommonScaleMain, TypeA) { } }
            public class B : Star { public B(Vector2 position) : base(position, ContentRegistry.starB, CommonScaleMain, TypeB) { } }
        }

        public class Giants
        {
            public class K : Star { public K(Vector2 position) : base(position, ContentRegistry.starK, CommonScaleGiants, TypeK) { } }
            public class G : Star { public G(Vector2 position) : base(position, ContentRegistry.starG, CommonScaleGiants, TypeG) { } }
            public class F : Star { public F(Vector2 position) : base(position, ContentRegistry.starF, CommonScaleGiants, TypeF) { } }
        }

        public class SuperGiants
        {
            public class M : Star { public M(Vector2 position) : base(position, ContentRegistry.starM, CommonScaleSuperGiants, TypeM) { } }
            public class G : Star { public G(Vector2 position) : base(position, ContentRegistry.starG, CommonScaleSuperGiants, TypeG) { } }
            public class A : Star { public A(Vector2 position) : base(position, ContentRegistry.starA, CommonScaleSuperGiants, TypeA) { } }
            public class O : Star { public O(Vector2 position) : base(position, ContentRegistry.starO, CommonScaleSuperGiants, TypeO) { } }
        }
    }

}
