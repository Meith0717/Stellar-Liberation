
using System;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    public class NoiseMapGenerator
    {
        private PerlinNoise perlinNoise;
        private Random rnd;
        private int width;
        private int height;

        public NoiseMapGenerator(int seed, int width, int height)
        {
            perlinNoise = new PerlinNoise(seed);
            this.width = width;
            this.height = height;
            rnd = new Random(seed);
        }

        public int[,] GenerateBinaryNoiseMap(double scale, int octaves, double lacunarity, double persistence, double threshold)
        {
            int[,] noiseMap = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double sampleX = x / scale;
                    double sampleY = y / scale;
                    double noiseValue = perlinNoise.FractalNoise(sampleX, sampleY, octaves, lacunarity, persistence);


                    // Thresholding
                    if (noiseValue >= threshold)
                        noiseMap[x, y] = 1;
                    else
                        noiseMap[x, y] = 0;
                }
            }

            return noiseMap;
        }

        public int[,] GenerateNoiseMap(double scale, int octaves, double lacunarity, double persistence, double threshold)
        {
            int[,] noiseMap = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double sampleX = x / scale;
                    double sampleY = y / scale;
                    double noiseValue = perlinNoise.FractalNoise(sampleX, sampleY, octaves, lacunarity, persistence);

                    // Map the noise value to the desired range
                    int mappedValue = HashValue(noiseValue);

                    // Thresholding
                    if (mappedValue >= threshold)
                        noiseMap[x, y] = mappedValue;
                    else
                        noiseMap[x, y] = 0;
                }
            }

            // Post-processing to ensure pixels with value 1 do not have neighbors with value 1 (optional)

            return noiseMap;
        }

        // Helper function to map a noise value to the desired range
        private int HashValue(double value)
        {
            int numValues = 8;
            double range = 1.0 / numValues;

            int hash = (int)Math.Floor(value / range) + 1;
            return Math.Min(hash, numValues);
        }
    }
}
