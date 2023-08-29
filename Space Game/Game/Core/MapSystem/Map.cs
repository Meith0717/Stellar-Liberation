using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Graph;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.MapSystem
{
    [Serializable]
    public class Map
    {
        [JsonProperty] private int mSectorCountWidth = 20;
        [JsonProperty] private int mSectorCountHeight = 20;
        [JsonProperty] private int mSectorSize = 150;

        [JsonProperty] public UndirectedGraph<SolarSystem> mGraphMap = new();

        public int Height { get { return mSectorCountHeight * mSectorSize; } }
        public int Width { get { return mSectorCountWidth * mSectorSize; } }
        public Vector2 Middle { get { return new(Width / 2, Height / 2); } }

        public void Generate(GameLayer gameLayer)
        {
            NoiseMapGenerator noiseMapGenerator 
                = new(RandomSeed.Time(), mSectorCountWidth, mSectorCountHeight);
            var noiseMap = noiseMapGenerator.GenerateBinaryNoiseMap(40, 6, 5, 0.85, 0);

            SystemGenerator.Generate(noiseMap, mSectorSize, gameLayer, out var systems);

            foreach (SolarSystem system in systems)
            {
                gameLayer.AddObject(system);
                mGraphMap.AddVertex(system);
            }

            foreach (SolarSystem system in mGraphMap.Vertices)
            {
                var neighbors = gameLayer.GetSortedObjectsInRadius<SolarSystem>(system.Position, 10000);
                neighbors.Remove(system);
                var amount = Utility.Utility.Random.Next(1, 5);
                amount = (neighbors.Count < amount) ? neighbors.Count : amount;
                for (int i = 0; i < amount; i++)
                {
                    var neighbor = neighbors[i];
                    var edge = new UndirectedEdge<SolarSystem>(system, neighbor);
                    if (mGraphMap.Edges.Contains(edge)) continue;
                    mGraphMap.AddEdge(edge);
                }
            }
        }

        public SolarSystem GetRandomSystem()
        {
            return Utility.Utility.GetRandomElement(mGraphMap.Vertices.ToList());
        }

        public bool GetActualSystem(out SolarSystem system)
        {
            system = null;
            foreach (var s in mGraphMap.Vertices.OfType<SolarSystem>())
            {
                if (!s.HasPlayer) continue;
                system = s;
                return true;
            }
            return false;
        }

        public List<SolarSystem> GetPath(SolarSystem source, SolarSystem target)
        {
            var path = mGraphMap.Dijkstra(source, target);
            foreach (var s in path)
            {
                s.IsPath = true;
            }
            return path;
        }

        public void DrawEdges()
        {
            foreach (var edge in mGraphMap.Edges)
            {
                TextureManager.Instance.DrawLine(edge.Source.Position, edge.Target.Position, new(50, 50, 50, 50), 2, 0);
            }
        }

    }
}
