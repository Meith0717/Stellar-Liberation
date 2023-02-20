using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Game.GameObjects;
using System;

namespace Space_Game.Core.GameObject
{
    public abstract class MovableObject : GameObject
    {
        [JsonProperty] public float Velocity;
        [JsonProperty] public bool IsMoving;
        [JsonProperty] public Vector2 TargetPoint;
        [JsonProperty] public float Rotation;

        [JsonProperty] private Vector2 DirectionVector;
        [JsonProperty] private float TextureRotation;

        public override void Draw()
        {
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(Position, TargetPoint, Color.Red,  2 / Globals.mCamera2d.mZoom);
        }

        public void SetTarget(Vector2 targetPoint)
        {
            TargetPoint = targetPoint;
            DirectionVector = targetPoint - Position;
            DirectionVector.Normalize();
            TextureRotation = (float)Math.Acos(Vector2.Dot(new Vector2(1, 0), DirectionVector) / DirectionVector.Length());
            if (DirectionVector.Y < 0) { TextureRotation = -TextureRotation; }
        }

        public void Move(GameTime gameTime)
        {
            if (Position == TargetPoint) { return; }
            if (Vector2.Distance(Position, TargetPoint) <= 5) 
            {
                Position = TargetPoint;
                IsMoving = false;
                return;
            }

            DirectionVector = TargetPoint - Position;
            DirectionVector.Normalize();

            Rotation = (float)Math.Acos(Vector2.Dot(new Vector2(1, 0), DirectionVector) / DirectionVector.Length());
            if (DirectionVector.Y < 0) { Rotation = -Rotation; }

            Position += DirectionVector * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            IsMoving = true;
        }

        public void StopMoving()
        {
            IsMoving = false;
            TargetPoint = Position;
        }
    }
}

