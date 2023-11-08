// Turret.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class Turret : GameObject
    {
        private Vector2 mRelativePosition;

        public Turret(Vector2 relativePosition, float textureScale, int textureDepth)
        : base(Vector2.Zero, ContentRegistry.turette, textureScale, textureDepth) => mRelativePosition = relativePosition;

        public void Fire(ProjectileManager projectileManager, SpaceShip origin, Color particleColor, int shieldDamage, int hullDamage) => projectileManager.AddProjectiel(new(Position, Rotation, particleColor, shieldDamage, hullDamage, origin));


        public void GetPosition(Vector2 originPosition, float originRotation) => Position = Transformations.Rotation(originPosition, mRelativePosition, originRotation);

        public void RotateToTArget(float originRotation, Vector2? targetPosition) => Rotation += targetPosition switch
        {
            null => MovementController.GetRotationUpdate(Rotation, originRotation, 1),
            not null => MovementController.GetRotationUpdate(Rotation, Position, (Vector2)targetPosition) * 0.1f,
        };


        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}