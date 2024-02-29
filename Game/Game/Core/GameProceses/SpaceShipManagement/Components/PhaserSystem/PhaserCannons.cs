// PhaserCannons.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PhaserSystem
{
    public class PhaserCannons
    {
        private readonly List<PhaserCannon> mCannons = new();
        private readonly int mMaxFireCoolDown;
        private float mFireCoolDown;
        private Color mParticleColor;
        private float mHullDamage;
        private float mShielDamage;
        public float Range;

        private SpaceShip mAimingShip;
        private Vector2? mAimingPos;
        private bool mFire;

        public PhaserCannons(int fireCoolDown, Color particleColor, int hullDamage, int shieldDamage, float range)
        {
            mMaxFireCoolDown = fireCoolDown;
            mFireCoolDown = fireCoolDown;
            mHullDamage = hullDamage;
            mShielDamage = shieldDamage;
            mParticleColor = particleColor;
            Range = range;
        }

        public void PlaceTurret(PhaserCannon turret) => mCannons.Add(turret);

        public SpaceShip AimingShip => mAimingShip;
        public void AimShip(SpaceShip spaceShip) => mAimingShip = spaceShip;
        public void AimPosition(Vector2? position) => mAimingPos = position;
        public void Fire() => mFire = true;
        public void StopFire() => mFire = false;

        public void Update(GameTime gameTime, SpaceShip origin, GameLayer scene)
        {
            mFireCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            var hasFired = false;
            foreach (var cannon in mCannons)
            {
                var position = mAimingPos ?? CollisionPredictor.PredictPosition(gameTime, origin.Position, 15, mAimingShip);
                cannon.GetPosition(origin.Position, origin.Rotation);
                cannon.RotateToTArget(origin.Rotation, position);
                if (!mFire || mFireCoolDown < mMaxFireCoolDown) continue;
                cannon.Fire(scene.ParticleManager, origin, mParticleColor, mShielDamage, mHullDamage);
                hasFired = true;
            }

            if (hasFired) SoundEffectSystem.PlaySound(SoundEffectRegistries.torpedoFire, scene.Camera2D, origin.Position);
            if (hasFired) mFireCoolDown = 0;
        }

        public void ControlByInput(InputState inputState, Vector2 worldMousePosition)
        {
            StopFire();
            AimPosition(null);
            inputState.DoAction(ActionType.LeftClickHold, () => Fire());
        }

        public void Upgrade(float hullDamagePercentage = 0, float shieldDamagePercentage = 0, float fireCoolDownPercentage = 0)
        {
            mHullDamage += mHullDamage * hullDamagePercentage;
            mShielDamage += mShielDamage * shieldDamagePercentage;
            mFireCoolDown -= mFireCoolDown * fireCoolDownPercentage;
        }

        public void Draw(GameLayer sceme) { foreach (var weapon in mCannons) weapon.Draw(sceme); }
    }
}
