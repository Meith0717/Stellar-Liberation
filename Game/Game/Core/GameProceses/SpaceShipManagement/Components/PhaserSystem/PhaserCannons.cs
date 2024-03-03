// PhaserCannons.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PhaserSystem
{
    public class PhaserCannons
    {
        private readonly List<PhaserCannon> mCannons = new();
        private readonly int mMaxFireCoolDown;
        private readonly Color mParticleColor;
        private readonly float mHullDamage;
        private readonly float mShielDamage;
        private bool mFire;
        private float mFireCoolDown = 0;

        public PhaserCannons(int fireCoolDown, Color particleColor, int hullDamage, int shieldDamage)
        {
            mMaxFireCoolDown = fireCoolDown;
            mHullDamage = hullDamage;
            mShielDamage = shieldDamage;
            mParticleColor = particleColor;
        }

        public void PlaceTurret(PhaserCannon turret) => mCannons.Add(turret);

        public void Fire() => mFire = true;
        public void StopFire() => mFire = false;

        public void Update(GameTime gameTime, SpaceShip origin, GameLayer scene)
        {
            mFireCoolDown -= gameTime.ElapsedGameTime.Milliseconds;

            foreach (var cannon in mCannons)
                cannon.GetPosition(origin.Position, origin.Rotation, origin.Rotation);
            if (!mFire || mFireCoolDown > 0) return;
            foreach (var cannon in mCannons)
                cannon.Fire(origin.PlanetSystem.GameObjects, origin, mParticleColor, mShielDamage, mHullDamage);
            SoundEffectSystem.PlaySound(SoundEffectRegistries.torpedoFire, scene.Camera2D, origin.Position);
            mFireCoolDown = mMaxFireCoolDown;
        }

        public void Draw(GameLayer sceme) { foreach (var weapon in mCannons) weapon.Draw(sceme); }
    }
}
