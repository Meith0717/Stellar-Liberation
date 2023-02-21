using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using NVorbis;
using rache_der_reti.Core.TextureManagement;
using System;

namespace Space_Game.Core.GameObject
{
    public abstract class MovableObject : GameObject
    {
        [JsonProperty] public float Velocity;
        [JsonProperty] public bool IsMoving;
        [JsonProperty] public Vector2 TargetPoint;
        [JsonProperty] public Vector2 StartPoint;
        [JsonProperty] public float Rotation;
        [JsonProperty] public float TravelTime;

        [JsonProperty] private Vector2 DirectionVector;
        [JsonProperty] private float TextureRotation;

        public override void Draw()
        {
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(Position, TargetPoint, Color.Red,  2 / Globals.mCamera2d.mZoom);
        }
                                                                      
        public void SetTarget(Vector2 targetPoint)
        {
            TargetPoint = targetPoint;
            StartPoint = Position;
            DirectionVector = targetPoint - Position;
            DirectionVector.Normalize();
            TextureRotation = (float)Math.Acos(Vector2.Dot(new Vector2(1, 0), DirectionVector) / DirectionVector.Length());
            if (DirectionVector.Y < 0) { TextureRotation = -TextureRotation; }
        }

        public void Move(GameTime gameTime)
        {
            var speed = Globals.mTimeWarp * Velocity;

            if (Position == TargetPoint) { return; }

            float distanceToTarget = Vector2.Distance(Position, TargetPoint);

            if (distanceToTarget <= 5) 
            {
                Position = TargetPoint;
                IsMoving = false;
                return;
            }

            TravelTime = (distanceToTarget / Velocity)/1000;
            DirectionVector = TargetPoint - Position;
            DirectionVector.Normalize();
            Rotation = MyMathF.GetInstance().GetRotation(DirectionVector);
            Position += DirectionVector * speed * gameTime.ElapsedGameTime.Milliseconds;
            IsMoving = true;
        }

        public void StopMoving()
        {
            IsMoving = false;
            TargetPoint = Position;
        }
    }
}

