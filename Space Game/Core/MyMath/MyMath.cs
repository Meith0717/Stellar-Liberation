using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace Galaxy_Explovive.Core.MyMath
{
    public class MyMath
    {
        private static MyMath sInstance;
        public static MyMath Instance { get { return sInstance ??= new MyMath(); } }

        public int GetRandomInt(int randomVariance)
        {
            var i = Globals.mRandom.Next(-randomVariance, randomVariance);
            return i;
        }

        public Vector2 GetRandomVector2(Vector2 Origin)
        {

            return Origin + new Vector2(Globals.mRandom.Next(5000), Globals.mRandom.Next(5000));
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

        public string GetTimeFromSeconds(int seconds)
        {
            int days = seconds / 86400;
            seconds %= 86400;
            int hours = seconds / 3600;
            seconds %= 3600;
            int minutes = seconds / 60;
            seconds %= 60;
            return $"{days} Days - {ExpandIntToTwoStr(hours)}:{ExpandIntToTwoStr(minutes)}:{ExpandIntToTwoStr(seconds)}";
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
