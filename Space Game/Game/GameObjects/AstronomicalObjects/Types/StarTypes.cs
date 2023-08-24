using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types
{
    public static class StarTypes
    {
        public static Star GenerateRandomStar(Vector2 position)
        {
            Random random = new Random();

            List<Type> starTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Star).IsAssignableFrom(type) && type != typeof(Star))
                .ToList();

            int randomIndex = random.Next(0, starTypes.Count);

            Type selectedStarType = starTypes[randomIndex];
            ConstructorInfo constructor = selectedStarType.GetConstructor(new Type[] { typeof(Vector2) });

            if (constructor != null)
            {
                return (Star)constructor.Invoke(new object[] { position });
            }

            return null;
        }

        public static Color TypeM = new(255, 177, 110);
        public static Color TypeK = new(255, 218, 187);
        public static Color TypeG = new(255, 237, 222);
        public static Color TypeF = new(243, 242, 255);
        public static Color TypeA = new(210, 223, 255);
        public static Color TypeB = new(181, 205, 255);
        public static Color TypeO = new(97, 130, 253);

        public class Dwarf
        {
            private static float mScale = 10f;

            public class M : Star
            {
                public M(Vector2 position)
                    : base(position, ContentRegistry.starM, mScale, TypeM)
                {
                }
            }

            public class K : Star
            {
                public K(Vector2 position)
                    : base(position, ContentRegistry.starK, mScale, TypeK)
                {
                }
            }

            public class A : Star
            {
                public A(Vector2 position)
                    : base(position, ContentRegistry.starA, mScale, TypeA)
                {
                }
            }
        }

        public class Main
        {
            private static float mScale = 15f;

            public class G : Star
            {
                public G(Vector2 position)
                    : base(position, ContentRegistry.starG, mScale, TypeG)
                {
                }
            }

            public class F : Star
            {
                public F(Vector2 position)
                    : base(position, ContentRegistry.starF, mScale, TypeF)
                {
                }
            }

            public class A : Star
            {
                public A(Vector2 position)
                    : base(position, ContentRegistry.starA, mScale, TypeA)
                {
                }
            }

            public class B : Star
            {
                public B(Vector2 position)
                    : base(position, ContentRegistry.starB, mScale, TypeB)
                {
                }
            }
        }

        public class Giants
        {
            private static float mScale = 20f;

            public class K : Star
            {
                public K(Vector2 position)
                    : base(position, ContentRegistry.starK, mScale, TypeK)
                {
                }
            }

            public class G : Star
            {
                public G(Vector2 position)
                    : base(position, ContentRegistry.starG, mScale, TypeG)
                {
                }
            }

            public class F : Star
            {
                public F(Vector2 position)
                    : base(position, ContentRegistry.starF, mScale, TypeF)
                {
                }
            }
        }

        public class SuperGiants
        {
            private static float mScale = 25f;

            public class M : Star
            {
                public M(Vector2 position)
                    : base(position, ContentRegistry.starM, mScale, TypeM)
                {
                }
            }

            public class G : Star
            {
                public G(Vector2 position)
                    : base(position, ContentRegistry.starG, mScale, TypeG)
                {
                }
            }

            public class A : Star
            {
                public A(Vector2 position)
                    : base(position, ContentRegistry.starA, mScale, TypeA)
                {
                }
            }

            public class O : Star
            {
                public O(Vector2 position)
                    : base(position, ContentRegistry.starO, mScale, TypeO)
                {
                }
            }
        }
    }
}
