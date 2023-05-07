using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Galaxy_Explovive.Core.RayTracing
{
    public class Ray
    {
        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        private float Angle { get; set; }

        public Ray(Vector2 startPosition, Vector2 endPosition, float anlge)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Angle = anlge;
        }

        public Vector2 GetPositionFromRadius(float radius)
        { 
            CircleF radiusCircle = new CircleF(StartPosition, radius);
            return (Vector2)radiusCircle.BoundaryPointAt(Angle); 
        }
    }
}
