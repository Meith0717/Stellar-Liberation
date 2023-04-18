using Microsoft.Xna.Framework;
using System;

namespace Galaxy_Explovive.Core.Maths
{
    internal class MyMathF
    {
        private static MyMathF sInstance;

        public static MyMathF GetInstance()
        {
            return sInstance ??= new MyMathF();
        }

        public int GetRandomInt(int randomVariance)
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

        public float GetRotation(Vector2 position, Vector2 target)
        {
            Vector2 directionVector = target - position;
            float rotation = (float)MathF.Acos(Vector2.Dot(new Vector2(1, 0), directionVector) / directionVector.Length());
            if (directionVector.Y < 0) { rotation = 2*MathF.PI - MathF.Abs(rotation); }
            return rotation;
        }

        public Vector2 GetDirection(float rotation)
        {
            return new Vector2(MathF.Cos(rotation), MathF.Sin(rotation));
        }

        public string GetTimeFromSekonds(int sekonds)
        {
            int days = sekonds / 86400;
            sekonds %= 86400;
            int houers = sekonds / 3600;
            sekonds %= 3600;
            int minutes = sekonds / 60;
            sekonds %= 60;
            return $"{days} Days - {ExpandIntToTwoStr(houers)}:{ExpandIntToTwoStr(minutes)}:{ExpandIntToTwoStr(sekonds)}";
        }

        private string ExpandIntToTwoStr(int str)
        {
            if (str.ToString().Length > 1)
            {
                return str.ToString();
            }
            return $"0{str}";
        }
    }
}
