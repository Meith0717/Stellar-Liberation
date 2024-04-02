﻿// PhaserCannon.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.BaseComponents.PhaserSystem
{
    [Obsolete]
    public class PhaserCannon : GameObject2D
    {
        private Vector2 mRelativePosition;

        public PhaserCannon(Vector2 relativePosition, float textureScale, int textureDepth)
        : base(Vector2.Zero, GameSpriteRegistries.turette, textureScale, textureDepth)
            => mRelativePosition = relativePosition;

        public void Fire(PlanetsystemState planetsystemState, Spaceship origin, Color particleColor, float shieldDamage, float hullDamage)
        {
            //gameLayer.GameObjectsManager.Add(new LaserProjectile(Position, Rotation, particleColor, shieldDamage, hullDamage, origin.Fraction));
        }

        public void GetPosition(Vector2 originPosition, float originRotation, float rotation)
        {
            Position = Transformations.Rotation(originPosition, mRelativePosition, originRotation);
            Rotation = rotation;
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}