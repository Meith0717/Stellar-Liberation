﻿// Projectile.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Animations;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using rache_der_reti.Core.Animation;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class Projectile : MovingObject
    {
        public SpaceShip Origine { get; private set; }

        public readonly int ShieldDamage;
        public readonly int HullDamage;

        private readonly SpriteSheet mExplosionSheet;
        private bool mHit;

        public Projectile(Vector2 weaponPosition, float rotation, Color color, int shieldDamage, int hullDamage, SpaceShip origine)
            : base(weaponPosition, ContentRegistry.projectile, 0.5f, 20)
        {
            Direction = Geometry.CalculateDirectionVector(rotation);
            Rotation = rotation;
            HullDamage = hullDamage;
            ShieldDamage = shieldDamage;
            TextureColor = color;
            Origine = origine;
            Velocity = 15;
            DeleteTime = 5000;
            mExplosionSheet = new(ContentRegistry.explosion, 64, 3, TextureScale * 2);
            mExplosionSheet.Animate("hit", new(60, Animation.GetRowList(0, 64), false));
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            mExplosionSheet.Update(gameTime, Position);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            mExplosionSheet.Draw(TextureDepth + 1);
            if (!mHit) TextureManager.Instance.DrawGameObject(this);
        }

        public override void HasCollide()
        {
            LiveTime16 = 5000 - ((1000 / 60) * 64);
            Velocity = 0;
            mHit = true;
            mExplosionSheet.Play("hit");
        }
    }
}
