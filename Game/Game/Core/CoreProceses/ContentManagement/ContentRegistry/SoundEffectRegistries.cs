// SoundEffectRegistries.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class SoundEffectRegistries : Registries
    {
        private readonly static string soundEffects = @"sounds\";
        public readonly static Registry torpedoHit = new(soundEffects, "torpedoHit");
        public readonly static Registry torpedoFire = new(soundEffects, "torpedoFire");
        public readonly static Registry collect = new(soundEffects, "collect");
        public readonly static Registry ChargeHyperdrive = new(soundEffects, "chargeHyperdrive");
        public readonly static Registry CoolHyperdrive = new(soundEffects, "coolHyperdrive");
        public readonly static Registry click = new(soundEffects, "click");
        public readonly static Registry hover = new(soundEffects, "hover");
    }
}
