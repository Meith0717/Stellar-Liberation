using Galaxy_Explovive.Core.GameEngine;
using Galaxy_Explovive.Core.GameEngine.GameObjects;
using Galaxy_Explovive.Core.GameEngine.InputManagement;
using Galaxy_Explovive.Core.GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    /// <summary>
    /// Abstract class representing a spacecraft, derived from the InteractiveObject class.
    /// </summary>
    [Serializable]
    public abstract class Spacecraft : InteractiveObject
    {

        /// <summary>
        /// Gets or sets the shield force of the spacecraft.
        /// </summary>
        [JsonProperty]
        public double ShieldForce { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the hull force of the spacecraft.
        /// </summary>
        [JsonProperty]
        public double HullForce { get; set; } = 0.1f;

        [JsonProperty]
        private double mShield = 100;

        [JsonProperty]
        private double mHull = 100;

        /// <summary>
        /// Updates the logic of the spacecraft, including regeneration of shield and hull.
        /// </summary>
        /// <param name="gameTime">The game time information.</param>
        /// <param name="inputState">The input state of the game.</param>
        /// <param name="engine">The game engine instance.</param>
        public override void UpdateLogic(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogic(gameTime, inputState, engine);
            RegenerateShield();
            RegenerateHull();
        }

        /// <summary>
        /// Regenerates the shield of the spacecraft.
        /// </summary>
        private void RegenerateShield()
        {
            if (mHull == 0) { return; }
            double reg = 1 * ShieldForce;
            if (mShield >= 100) { mShield = 100; return; }
            mShield += reg;
        }

        /// <summary>
        /// Regenerates the hull of the spacecraft.
        /// </summary>
        private void RegenerateHull()
        {
            if (mHull == 0) { return; }
            double reg = 1 * HullForce;
            if (mHull >= 100) { mHull = 100; return; }
            mHull += reg;
        }

        /// <summary>
        /// Inflicts damage to the spacecraft.
        /// </summary>
        /// <param name="damage">The amount of damage to inflict.</param>
        public void Hit(int damage)
        {
            double ShieldDamage = damage * (1 - ShieldForce);
            double HullDamage = damage * (1 - HullForce);

            if (mShield - ShieldDamage > 0.01f)
            {
                mShield -= ShieldDamage;
                return;
            }
            mShield = 0;
            if (mHull - HullDamage < 0.01f) { mHull = 0; return; }
            mHull -= HullDamage;
        }

        /// <summary>
        /// Draws the spacecraft on the screen, including the shield and hull indicators.
        /// </summary>
        /// <param name="textureManager">The texture manager for managing textures.</param>
        /// <param name="engine">The game engine instance.</param>
        public override void Draw(TextureManager textureManager, GameEngine engine)
        {
            base.Draw(textureManager, engine);
            Vector2 startPos = Position - TextureOffset - new Vector2(0, 50);
            Vector2 endPos = new(TextureOffset.X * 2, 0);
            float hull = (float)mHull / 100;
            float shield = (float)mShield / 100;
            textureManager.DrawLine(startPos, startPos + endPos * new Vector2(shield, 1), Color.CornflowerBlue, 8, 1);
            textureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);
            startPos.Y += 10;
            textureManager.DrawLine(startPos, startPos + endPos * new Vector2(hull, 1), Color.GreenYellow, 8, 1);
            textureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);
        }
    }
}
