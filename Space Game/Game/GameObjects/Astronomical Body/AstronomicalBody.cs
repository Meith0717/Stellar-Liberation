using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using GalaxyExplovive.Core.GameEngine.InputManagement;
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
        /// Performs actions related to the selection of the astronomical body.
        /// Is deselect when pressing anywhere or moving the camera.
        /// </summary>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="engine">The game engine instance.</param>
        public override void SelectActions(InputState inputState, GameEngine engine)
        {
            // Check if the camera was moved by the user or if left-click occurred outside the body's hover area
            base.SelectActions(inputState, engine);
        }
    }
}
