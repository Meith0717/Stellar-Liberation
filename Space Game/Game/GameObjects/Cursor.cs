using Microsoft.Xna.Framework;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core.GameObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Game.GameObjects
{
    public class Cursor : GameObject
    {
    
        public override void Draw()
        {
            var cursor = TextureManager.GetInstance().GetTexture("cursor");
            TextureManager.GetInstance().GetSpriteBatch().Draw(
                cursor, Position, null, Color.White, 0, Vector2.Zero, 
                0.05f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Position = inputState.mMousePosition.ToVector2();
            Debug.WriteLine(inputState.mMousePosition);
        }
    }
}
