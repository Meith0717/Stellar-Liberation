using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects
{
    [Serializable]
    public abstract class SpaceShip : InteractiveObject
    {
        public SpaceShip(Vector2 position, string textureId, float textureScale, int textureDepth) 
            : base(position, textureId, textureScale, textureDepth) { }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine)
        {
            base.Update(gameTime, inputState, gameEngine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
        }

        
    }
}
