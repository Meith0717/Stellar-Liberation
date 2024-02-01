// Turret.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PhaserSystem
{
    public class PhaserCannon : GameObject2D
    {
        private Vector2 mRelativePosition;

        public PhaserCannon(Vector2 relativePosition, float textureScale, int textureDepth)
        : base(Vector2.Zero, GameSpriteRegistries.turette, textureScale, textureDepth) => mRelativePosition = relativePosition;

        public void Fire(GameObjectManager objManager, SpaceShip origin, Color particleColor, float shieldDamage, float hullDamage) => objManager.AddObj(new LaserProjectile(this.Position, Rotation, particleColor, shieldDamage, hullDamage, origin.Fraction));

        public void GetPosition(Vector2 originPosition, float originRotation) => Position = Transformations.Rotation(originPosition, mRelativePosition, originRotation);

        public void RotateToTArget(float originRotation, Vector2? targetPosition) => Rotation += targetPosition switch
        {
            null => MovementController.GetRotationUpdate(Rotation, originRotation, 1),
            not null => MovementController.GetRotationUpdate(Rotation, Position, (Vector2)targetPosition) * 0.5f,
        };

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}