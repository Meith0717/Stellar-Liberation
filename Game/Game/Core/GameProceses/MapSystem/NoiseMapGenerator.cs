// NoiseMapGenerator.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System;

namespace StellarLiberation.Game.Core.GameProceses.MapSystem
{
    public class NoiseMapGenerator
    {
        private int width;
        private int height;

        public NoiseMapGenerator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public int[,] GenerateBinaryNoiseMap()
        {
            int[,] matrix = new int[width, height];

            // Mittelpunkt des Kreises
            int centerX = height / 2;
            int centerY = width / 2;

            // Radius des Kreises (angepasst an die Matrixgröße)
            double radius = Math.Min(width, height) / 2.0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    // Berechnung der Entfernung zum Mittelpunkt
                    double distanceToCenter = Math.Sqrt(Math.Pow(i - centerY, 2) + Math.Pow(j - centerX, 2));

                    // Je geringer die Entfernung zum Mittelpunkt, desto wahrscheinlicher ist eine 1
                    double maxProbability = 0.9; // Max Wahrscheinlichkeit in der Mitte
                    double minProbability = 0.1; // Min Wahrscheinlichkeit an den Rändern

                    // Wahrscheinlichkeit basierend auf der Entfernung
                    double probability = minProbability + (maxProbability - minProbability) * (1 - distanceToCenter / radius);

                    // Zufällige Entscheidung basierend auf der Wahrscheinlichkeit
                    matrix[i, j] = new Random().NextDouble() < probability ? 1 : 0;
                }
            }

            return matrix;
        }
    }
}
