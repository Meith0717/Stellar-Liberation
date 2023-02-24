using Microsoft.Xna.Framework;
using Space_Game.Core.GameObject;
using Space_Game.Core.InputManagement;
using System;

namespace Space_Game.Game.GameObjects
{
    [Serializable]
    public class SpaceStation : GameObject
    {
        const int textureWidth = 188;
        const int textureHeight = 128;
 
        public SpaceStation(Vector2 position) 
        {
        Position = position;
        TextureOffset = new Vector2(textureHeight, textureHeight) / 2;
        TextureId = "spaceStation";
        TextureSclae = 0.4f;
        TextureWidth = textureWidth;
        TextureHeight = textureHeight;
        TextureDepth = 0;
        TextureRotation = 0;
        TextureColor = Color.White;
        }

        public override void Draw() 
        {
            this.DrawGameObject();
        }

        public void Update(Vector2 position) { Position = position; }

        public override void Update(GameTime gameTime, InputState inputState) {  /* Do Nothing*/ }
    }
}
