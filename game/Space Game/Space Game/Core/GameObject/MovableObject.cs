using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Core.GameObject
{
    internal abstract class MovableObject : GameObject
    {
        [JsonProperty] public float Velocity;
        [JsonProperty] public Vector2 DirectionVector;
        [JsonProperty] public Vector2 TargetPoint;
        [JsonProperty] public float TextureRotation;
        [JsonProperty] public bool IsMoving;

        public override void Draw()
        {
            TextureManager.GetInstance().GetSpriteBatch().DrawLine(Position, TargetPoint, Color.LightBlue);
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
            if (Vector2.Distance(Position, TargetPoint) <= Velocity*10) 
            {
                Position = TargetPoint;
                IsMoving = false;
                return;
            }
            Position += DirectionVector * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            IsMoving = true;
        }
    }
}

