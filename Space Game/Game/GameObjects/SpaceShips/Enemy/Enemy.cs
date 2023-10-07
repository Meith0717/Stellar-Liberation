using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public abstract class Enemy : Spacecrafts.SpaceShip
    {
        protected BehaviorBasedAI mAi;

        protected Enemy(Vector2 position, string textureId, float textureScale) 
            : base(position, textureId, textureScale)
        { }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            mAi.Update(gameTime, SensorArray.SortedObjectsInRange, this);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            DefenseSystem.DrawLive(this);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
