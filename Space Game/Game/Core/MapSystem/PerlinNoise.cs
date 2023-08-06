using System;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public class PerlinNoise
    {
        private int[] permutation;

        public PerlinNoise(int seed)
        {
            permutation = new int[512];
            var random = new Random(seed);

            for (int i = 0; i < 256; i++)
            {
                permutation[i] = i;
            }

            for (int i = 0; i < 256; i++)
            {
                int j = random.Next(256);
                int temp = permutation[i];
                permutation[i] = permutation[j];
                permutation[j] = temp;
                permutation[i + 256] = permutation[i];
            }
        }

        private double Grad(int hash, double x, double y)
        {
            int h = hash & 15;
            double grad = 1 + (h & 7); // Gradient value from 1 to 8
            if ((h & 8) != 0) grad = -grad; // Random sign
            return (grad * x) + (grad * y);
        }

        private double Fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private double Lerp(double t, double a, double b)
        {
            return a + t * (b - a);
        }

        private double Noise(double x, double y)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);

            double u = Fade(x);
            double v = Fade(y);

            int A = permutation[X] + Y;
            int AA = permutation[A];
            int AB = permutation[A + 1];
            int B = permutation[X + 1] + Y;
            int BA = permutation[B];
            int BB = permutation[B + 1];

            return Lerp(v, Lerp(u, Grad(permutation[AA], x, y),
                                  Grad(permutation[BA], x - 1, y)),
                            Lerp(u, Grad(permutation[AB], x, y - 1),
                                  Grad(permutation[BB], x - 1, y - 1)));
        }

        public double FractalNoise(double x, double y, int octaves, double lacunarity, double persistence)
        {
            double sum = 0;
            double frequency = 1;
            double amplitude = 1;
            double maxValue = 0;

            for (int i = 0; i < octaves; i++)
            {
                sum += Noise(x * frequency, y * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return sum / maxValue;
        }
    }
}
