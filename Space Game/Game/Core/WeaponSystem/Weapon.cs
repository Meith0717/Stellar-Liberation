using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core.WeaponSystem
{
    public abstract class Weapon : GameObject
    {
        public float LiveTime;

        public Weapon(Vector2 position, string textureId, float textureScale, int textureDepth) : base(position, textureId, textureScale, textureDepth)
        {
        }

        internal Weapon(Vector2 position, string textureId, float textureScale, int textureDepth, float liveTime)
            : base(position, textureId, textureScale, textureDepth)
        {
            LiveTime = liveTime;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            base.Update(gameTime, inputState, engine);
        }
    }
}
