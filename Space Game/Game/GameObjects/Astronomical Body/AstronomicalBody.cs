using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System;

namespace GalaxyExplovive.Game.GameObjects.Astronomical_Body
{
    /// <summary>
    /// Abstract class representing an astronomical body, derived from the InteractiveObject class.
    /// </summary>
    [Serializable]
    public abstract class AstronomicalBody : InteractiveObject
    {
        /// <summary>
        /// </summary>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="engine">The game engine instance.</param>
        public override void Update(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            // Check if the camera was moved by the user or if left-click occurred outside the body's hover area
            base.Update(gameTime, inputState, engine);
        }
    }
}
