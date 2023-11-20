// Particle.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.InputManagement;

namespace StellarLiberation.Game.Core.ParticleSystem
{
    public class Particle : GameObject2D
    {

        public Particle(Vector2 position, Vector2 movementDirection, float textureScale, float velocity, Color startColor, double dispodeTime)
            : base(position, "particle", textureScale, 1)
        {
            MovingDirection = movementDirection;
            Velocity = velocity;
            TextureColor = startColor;
            DisposeTime  = dispodeTime;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
