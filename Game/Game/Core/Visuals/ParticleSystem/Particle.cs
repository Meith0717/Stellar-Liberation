// Particle.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem
{
    public class Particle : GameObject2D
    {

        public Particle(Vector2 position, Vector2 movementDirection, float textureScale, float velocity, Color startColor, double dispodeTime)
            : base(position, "particle", textureScale, 9)
        {
            MovingDirection = movementDirection;
            Velocity = velocity;
            TextureColor = startColor;
            DisposeTime = dispodeTime;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            GameObject2DMover.Move(gameTime, this, null);
            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
