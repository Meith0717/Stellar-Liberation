// NoiseMapGenerator.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    public static class BinaryMapGenerator
    {
        public static int[,] Generate(int width, int height, Random seededRandom)
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
                    matrix[i, j] = (distanceToCenter < radius) ? (seededRandom.NextDouble() < .2 ? 0 : 1) : 0;
                }
            }
            return matrix;
        }

        public static List<Vector2> GetVector2sFormBinaryMap(int[,] noiseMap)
        {
            var list = new List<Vector2>();

            int rows = noiseMap.GetLength(0);
            int columns = noiseMap.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (noiseMap[x, y] == 0) continue;
                    list.Add(new Vector2(x, y));
                }
            }
            return list;
        }

        public static void ScaleVector2s(ref List<Vector2> list, float scaling)
        {
            for (int i = 0; i < list.Count; i++) list[i] *= scaling;
        }

        public static void ShiftVector2s(ref List<Vector2> list, int shiftLength, Random seededRandom)
        {
            for (int i = 0; i < list.Count; i++)
            {
                seededRandom.NextUnitVector(out var shiftingDirection);
                list[i] += shiftingDirection * shiftLength ;
            }
        }
    }
}
