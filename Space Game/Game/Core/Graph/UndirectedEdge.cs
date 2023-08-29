using System.Collections.Generic;
using System.Drawing;

namespace CelestialOdyssey.Game.Core.Graph
{
    public class UndirectedEdge<T>
    {
        public T Source { get; }
        public T Target { get; }

        public Color EdgeColor = Color.White;

        public UndirectedEdge(T source, T target)
        {
            Source = source;
            Target = target;
        }

        public override bool Equals(object obj)
        {
            if (obj is UndirectedEdge<T> otherEdge)
            {
                return (EqualityComparer<T>.Default.Equals(Source, otherEdge.Source) &&
                        EqualityComparer<T>.Default.Equals(Target, otherEdge.Target)) ||
                       (EqualityComparer<T>.Default.Equals(Source, otherEdge.Target) &&
                        EqualityComparer<T>.Default.Equals(Target, otherEdge.Source));
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashSource = Source.GetHashCode();
            int hashTarget = Target.GetHashCode();
            return hashSource ^ hashTarget;
        }
    }
}
