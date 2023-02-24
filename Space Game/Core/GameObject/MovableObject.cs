using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Space_Game.Core.Maths;
using Space_Game.Core.TextureManagement;

namespace Space_Game.Core.GameObject
{
    public abstract class MovableObject : GameObject
    {
        // Texture Stuff
        [JsonProperty] public float TravelTime;

        // Moving Stuff
        [JsonProperty] public Vector2 TargetPoint;
        [JsonProperty] public float Velocity;

        // bool
        [JsonProperty] public bool IsMoving;

        public void DrawPath(Color color)
        {
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(Position, TargetPoint, color, 2 / Globals.mCamera2d.mZoom);
        }

        public void SetTarget(Vector2 targetPoint)
        {
            TargetPoint = targetPoint;
        }

        public void Move(GameTime gameTime)
        {
            IsMoving = false;

            if (Position == TargetPoint) { return; }

            // Distance Stuff
            float distanceToTarget = Vector2.Distance(Position, TargetPoint);
            TravelTime = (distanceToTarget / Velocity) / 1000;
            if (distanceToTarget <= 5)
            {
                Position = TargetPoint;
                return;
            }

            // Position Stuff
            Globals.mGameLayer.mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
            Vector2 direction = (TargetPoint - Position).NormalizedCopy();
            TextureRotation = MyMathF.GetInstance().GetRotation(direction);
            Position += direction * Globals.mTimeWarp * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            Globals.mGameLayer.mSpatialHashing.InsertObject(this, (int)Position.X, (int)Position.Y);
            IsMoving = true;
        }

        public void StopMoving()
        {
            IsMoving = false;
            TargetPoint = Position;
        }

        public void RemoveFromSpatialHashing()
        {
            Globals.mGameLayer.mSpatialHashing.RemoveObject(this, (int)Position.X, (int)Position.Y);
        }
    }
}

