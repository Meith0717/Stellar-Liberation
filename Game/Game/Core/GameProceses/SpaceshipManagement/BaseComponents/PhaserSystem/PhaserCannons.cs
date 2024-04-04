// PhaserCannons.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.BaseComponents.PhaserSystem
{
    [Obsolete]
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

        public void Update(GameTime gameTime, Spaceship origin, PlanetsystemState planetsystemState)
        {
            mFireCoolDown -= gameTime.ElapsedGameTime.Milliseconds;

            foreach (var cannon in mCannons)
                cannon.GetPosition(origin.Position, origin.Rotation, origin.Rotation);
            if (!mFire || mFireCoolDown > 0) return;
            foreach (var cannon in mCannons)
                cannon.Fire(planetsystemState, origin, mParticleColor, mShielDamage, mHullDamage);
            planetsystemState.StereoSounds.Enqueue(new(origin.Position, SoundEffectRegistries.torpedoFire));
            mFireCoolDown = mMaxFireCoolDown;
        }

        public void Draw(GameState gameState, GameLayer sceme)
        {
            foreach (var weapon in mCannons)
                weapon.Draw(gameState, sceme);
        }
    }
}
