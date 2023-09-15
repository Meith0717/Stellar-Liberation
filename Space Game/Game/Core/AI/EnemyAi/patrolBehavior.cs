using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace CelestialOdyssey.Game.Core.AI.EnemyAi
{
    internal class patrolBehavior
    {

        private const float mMinDistance = 100;
        private Vector2? mTarget = null;


        public void Update(SpaceShip spaceShip, SceneLayer sceneLayer)
        {
            if (mTarget == null)
            {
                var targets = sceneLayer.GetObjectsInRadius<Planet>(spaceShip.Position, 10000000);
                if (targets.Count <= 0) return;
                var planet = Utility.Utility.GetRandomElement(targets);
                var angle = Geometry.AngleBetweenVectors(planet.Position, spaceShip.Position);
                mTarget = Geometry.GetPointOnCircle(planet.BoundedBox, angle);
                return;
            }
            spaceShip.Rotation += MovementController.GetRotationUpdate(spaceShip.Rotation, spaceShip.Position, (Vector2)mTarget, 0.1f);
            mTarget = (Vector2.Distance(spaceShip.Position, (Vector2)mTarget) < 10000) ? null : mTarget; 
        }
    }
}
