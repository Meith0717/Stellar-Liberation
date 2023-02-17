using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Core.GameObject;
using System;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class Planet : GameObject
    {
        const int textureHeight = 256;
        const int textureWidth = 256;
        const int scale = 5;

        public int mRadius;
        public int mAngleDegrees;
        
        public Planet(int radius, int angleDgrees) 
        {
            mRadius = radius;
            mAngleDegrees = angleDgrees;
            Offset = new Vector2(textureWidth, textureHeight) / 2 / scale;
            TextureId = "Planet";
            TextureHeight = textureHeight / scale;
            TextureWidth = textureWidth / scale;
        }

        public void SetPosition(Vector2 position)
        {
            float newX = mRadius * MathF.Cos(mAngleDegrees);
            float newY = mRadius * MathF.Sin(mAngleDegrees);

            Position = position + new Vector2(newX, newY);
        }

        public override void Draw()
        {
            TextureManager.GetInstance().Draw(TextureId, Position, TextureWidth, TextureHeight);
        }

        public override void Update(GameTime gameTime, InputState inputState, Camera2d camera2d)
        {
            // Do Nothing
        }
    }
}
