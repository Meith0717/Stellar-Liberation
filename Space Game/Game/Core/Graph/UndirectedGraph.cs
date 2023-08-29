using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.Graph
{
    [Serializable]
    public class UndirectedGraph<T>
    {
        [JsonProperty]
        public HashSet<T> Vertices { get; private set; }

        [JsonProperty]
        public HashSet<UndirectedEdge<T>> Edges { get; private set; }

        public UndirectedGraph()
        {
            Vertices = new();
            Edges = new();
        }

        public void AddVertex(T vertex)
        {
            Vertices.Add(vertex);
        }

        public void AddEdge(UndirectedEdge<T> edge)
        {
            Edges.Add(edge);
        }


        public List<T> Dijkstra(T start, T end)
        {
            if (!Vertices.Contains(start) || !Vertices.Contains(end))
            {
                throw new ArgumentException("Start or end vertex is not in the graph.");
            }

            Dictionary<T, double> distances = new Dictionary<T, double>();
            Dictionary<T, T> previousVertices = new Dictionary<T, T>();
            PriorityQueue<T, double> priorityQueue = new PriorityQueue<T, double>();

            foreach (var vertex in Vertices)
            {
                distances[vertex] = double.PositiveInfinity;
                previousVertices[vertex] = default;
            }

            distances[start] = 0;
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                var currentVertex = priorityQueue.Dequeue();
                if (EqualityComparer<T>.Default.Equals(currentVertex, end))
                {
                    break;  // Shortest path to the end vertex has been found
                }

                var currentVertexEdges = Edges.Where(edge =>
                    EqualityComparer<T>.Default.Equals(edge.Source, currentVertex) ||
                    EqualityComparer<T>.Default.Equals(edge.Target, currentVertex));

                foreach (var edge in currentVertexEdges)
                {
                    var neighbor = EqualityComparer<T>.Default.Equals(edge.Source, currentVertex)
                        ? edge.Target
                        : edge.Source;

                    var weight = 1.0;  // In a real scenario, you'd have edge weights

                    var newDistance = distances[currentVertex] + weight;

                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previousVertices[neighbor] = currentVertex;
                        priorityQueue.Enqueue(neighbor, newDistance);
                    }
                }
            }

            List<T> shortestPath = new List<T>();
            T current = end;

            while (!EqualityComparer<T>.Default.Equals(current, default))
            {
                shortestPath.Insert(0, current);
                current = previousVertices[current];
            }

            return shortestPath;
        }
    }
}
