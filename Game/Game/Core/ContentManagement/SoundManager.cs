// SoundManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Core.GameEngine.Content_Management
{
    public class SoundManager
    {
        private static SoundManager mInstance;

        public static SoundManager Instance
        {
            get
            {
                if (mInstance == null) { mInstance = new(); }
                return mInstance;
            }
        }

        private Hashtable SoundEffects { get; }
        private Hashtable SoundEffectInstances { get; }
        private int MaxSoundEffectInstances { get; }
        public float SoundVolume { get; set; } = 1.0f;
        public bool BackgroundMusicEnabled { get; set; } = true;
        public bool SoundEffectsEnabled { get; set; } = true;

        // config to sound good
        private readonly float mBackgroundMusicVolume = 0.7f;

        internal SoundManager(int maxSoundEffectInstances = 20)
        {
            SoundEffects = new Hashtable();
            SoundEffectInstances = new Hashtable();
            MaxSoundEffectInstances = maxSoundEffectInstances;
        }

        public void LoadRegistries(ContentManager content, List<Registry> registries)
        {
            foreach (Registry reg in registries) LoadSoundEffects(content, reg.Name, reg.FilePath);
        }

        private void LoadSoundEffects(ContentManager content, string id, string fileName)
        {
            if (SoundEffects[fileName] != null) SoundEffects.Remove(fileName);
            var soundEffect = content.Load<SoundEffect>(fileName);
            SoundEffects.Add(id, soundEffect);
        }

        public void CreateSoundEffectInstances()
        {
            ClearSoundEffectInstances();
            foreach (DictionaryEntry soundEffect in SoundEffects)
            {
                if (SoundEffectInstances[soundEffect.Key] != null)
                {
                    StopSound((string)soundEffect.Key);
                    SoundEffectInstances.Remove(soundEffect.Key);
                }
                var sfxInstances = new List<SoundEffectInstance>();
                for (var i = 0; i < MaxSoundEffectInstances; i++)
                {
                    var sfx = (SoundEffect)soundEffect.Value;

                    if (sfx == null)
                    {
                        continue;
                    }

                    var sfxInstance = sfx.CreateInstance();
                    sfxInstances.Add(sfxInstance);
                }
                SoundEffectInstances.Add(soundEffect.Key, sfxInstances);
            }
        }

        public void LoopBackgroundMusic(string backgroundMusic)
        {
            if (BackgroundMusicEnabled)
            {
                PlaySound(backgroundMusic, mBackgroundMusicVolume * SoundVolume, false, true, isBackroungMusic: true);
            }
        }

        public void StopBackgroundMusic(string backgroundMusic)
        {
            StopSound(backgroundMusic);
        }

        public void PlaySound(string key, float volume, bool allowInterrupt = false, bool isLooped = false, bool isBackroungMusic = false)
        {
            if (!SoundEffectInstances.ContainsKey(key))
            {
                return;
            }

            // dont play if no soundeffects are enabled
            if (!SoundEffectsEnabled && !isBackroungMusic)
            {
                return;
            }

            var instances = (List<SoundEffectInstance>)SoundEffectInstances[key];
            if (instances == null)
            {
                return;
            }
            // add volume setting to volume
            volume *= SoundVolume;

            // Make sure volume is between 0 and 1
            volume = volume <= 1 ? volume : 1;
            volume = volume >= 0 ? volume : 0;

            var instancesStopped = instances.Where(s => s.State == SoundState.Stopped);

            var soundEffectInstances = instancesStopped.ToList();

            SoundEffectInstance instance;
            switch (soundEffectInstances.Count)
            {
                case 0 when allowInterrupt:
                    {
                        instance = instances.First();
                        instance.Volume = volume;
                        instance.Stop();
                        instance.Play();
                        break;
                    }
                case > 0:
                    instance = soundEffectInstances.First();
                    instance.Volume = volume;
                    instance.IsLooped = isLooped;
                    instance.Play();
                    break;
            }
        }

        public void StopSound(string key)
        {
            if (!SoundEffectInstances.ContainsKey(key))
            {
                return;
            }

            var instances = (List<SoundEffectInstance>)SoundEffectInstances[key];

            foreach (var instance in instances!.Where(instance => instance.State != SoundState.Stopped))
            {
                instance.IsLooped = false;
                instance.Stop();
            }
        }

        public void StopAllSounds()
        {
            foreach (string soundEffect in SoundEffects.Keys)
            {
                StopSound(soundEffect);
            }
        }

        private void ClearSoundEffectInstances()
        {
            foreach (DictionaryEntry soundEffectInstance in SoundEffectInstances)
            {
                StopSound((string)soundEffectInstance.Key);
            }
            SoundEffectInstances.Clear();
        }

        internal void ChangeOverallVolume(float sliderValue)
        {
            foreach (DictionaryEntry soundEffectInstance in SoundEffectInstances)
            {
                if (soundEffectInstance.Value == null)
                {
                    continue;
                }

                foreach (var instance in (List<SoundEffectInstance>)soundEffectInstance.Value)
                {
                    if (instance == null)
                    {
                        continue;
                    }

                    if (sliderValue >= 0 && sliderValue <= 1)
                    {
                        instance.Volume = sliderValue;
                    }
                }
            }

            SoundVolume = sliderValue;
        }

        public void PausePlayAllSounds(bool pause)
        {
            foreach (DictionaryEntry soundEffectInstance in SoundEffectInstances)
            {
                if (soundEffectInstance.Value == null)
                {
                    continue;
                }

                foreach (var instance in (List<SoundEffectInstance>)soundEffectInstance.Value)
                {
                    if (instance == null)
                    {
                        continue;
                    }

                    if (pause && instance.State == SoundState.Playing)
                    {
                        instance.Pause();
                    }
                    else if (!pause && instance.State == SoundState.Paused)
                    {
                        instance.Resume();
                    }
                }
            }
        }

        public void ChangeSoundInstanceVolume(string key, float volume)
        {
            if (volume < 0 && volume > 1)
            {
                return;
            }
            if (!SoundEffectInstances.ContainsKey(key))
            {
                return;
            }
            var instances = (List<SoundEffectInstance>)SoundEffectInstances[key];

            foreach (var instance in instances!.Where(instance => instance.State != SoundState.Stopped))
            {
                instance.Volume = volume * SoundVolume;
            }
        }
    }
}

