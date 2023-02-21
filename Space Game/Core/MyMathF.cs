﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using rache_der_reti.Core.TextureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Core
{
    internal class MyMathF
    {
        private static MyMathF sInstance;

        public static MyMathF GetInstance()
        {
            return sInstance ??= new MyMathF();
        }

        private int GetRandomInt(int randomVariance)
        {
            var i = Globals.mRandom.Next(-randomVariance, randomVariance);
            return i;
        }

        public Vector2 GetCirclePosition(float radius, float angleRad, int randomVariance)
        {
            float newX = (radius + GetRandomInt(randomVariance)) * MathF.Cos(angleRad);
            float newY = (radius + GetRandomInt(randomVariance)) * MathF.Sin(angleRad);
            return new Vector2(newX, newY);
        }

        public float GetRotation(Vector2 vector)
        {
            float rotation = (float)Math.Acos(Vector2.Dot(new Vector2(1, 0), vector) / vector.Length());
            if (vector.Y < 0) { rotation = -rotation; }
            return rotation;
        }

        public string GetTimeFromSekonds(float sekonds)
        {
            TimeSpan time = TimeSpan.FromSeconds(sekonds);
            return time.ToString(@"hh\:mm\:ss"); ;
        }
    }
}
