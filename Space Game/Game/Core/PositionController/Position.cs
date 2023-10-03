using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace CelestialOdyssey.Game.Core.PositionController
{
    public class Position
    {
        public readonly Vector2 Location;
        public readonly float Rotation;
        public Position(float x, float y, float rotation)
        {
            Location = new Vector2(x, y);
            Rotation = rotation;
        }

        public float Distance(Vector2 target) 
        {
            return Vector2.Distance(Location, target);
        }

        public Vector2 Direction(Vector2 target)
        {
            return Vector2.Subtract(Location, target).NormalizedCopy();
        }
    }
}
