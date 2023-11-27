// SoundManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Core.GameEngine.Content_Management
{
    public class SoundEffectManager
    {
        private static SoundEffectManager mInstance;
        public static SoundEffectManager Instance { get { return mInstance ??= new(); } }
        private readonly Dictionary<string, List<SoundEffectInstance>> SoundEffectInstances = new();
        private readonly int MaxSoundEffectInstances = new();
        public float OverallVolume = 1.0f;

        public SoundEffectManager(int maxSoundEffectInstances = 20) => MaxSoundEffectInstances = maxSoundEffectInstances;

        public void LoadRegistries(ContentManager content, List<Registry> registries)
        {
            foreach (Registry reg in registries) 
            {
                var soundEffect = content.Load<SoundEffect>(reg.FilePath);
                var sfxInstances = new List<SoundEffectInstance>();
                for (var i = 0; i < MaxSoundEffectInstances; i++) sfxInstances.Add(soundEffect.CreateInstance());
                SoundEffectInstances[reg.Name] = sfxInstances;
            };
        }

        public void PlaySound(string soundId, bool allowInterrupt = false)
        {
            if (!SoundEffectInstances.ContainsKey(soundId)) throw new Exception("SoundId not found!");

            var soundEffectInstances = SoundEffectInstances[soundId];

            var stopedSoundEffectInstances = soundEffectInstances.Where(s => s.State == SoundState.Stopped).ToList();

            SoundEffectInstance instance;
            switch (stopedSoundEffectInstances.Count)
            {
                case 0 when allowInterrupt:
                    instance = soundEffectInstances.First();
                    instance.Volume = OverallVolume;
                    instance.Stop();
                    instance.Play();
                    break;
                case > 0:
                    instance = stopedSoundEffectInstances.First();
                    instance.Volume = OverallVolume;
                    instance.Play();
                    break;
            }
        }


        public void StopAllSounds()
        {
            foreach (string soundEffect in SoundEffectInstances.Keys)
            {
                if (!SoundEffectInstances.ContainsKey(soundEffect)) return;

                var instances = SoundEffectInstances[soundEffect];

                foreach (var instance in instances!.Where(instance => instance.State != SoundState.Stopped))
                {
                    instance.IsLooped = false;
                    instance.Stop();
                }
            }
        }

        internal void ChangeOverallVolume(float sliderValue)
        {
            foreach (var instances in SoundEffectInstances.Values)
            {
                foreach (var instance in instances)
                {
                    if (sliderValue >= 0 && sliderValue <= 1) instance.Volume = sliderValue;
                }
            }
            OverallVolume = sliderValue;
        }
    }
}

