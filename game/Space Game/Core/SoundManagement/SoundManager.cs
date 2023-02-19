using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace rache_der_reti.Core.SoundManagement
{
    public class SoundManager
    {
        private Hashtable SoundEffects { get; }
        private Hashtable SoundEffectInstances { get; }
        private int MaxSoundEffectInstances { get; }  // Max number of concurrent playing sound instances of one specific sound


        // variables to set via settings menu
        public float SoundVolume { get; set; } = 1.0f;
        public bool BackgroundMusicEnabled { get; set; } = true;
        public bool SoundEffectsEnabled { get; set; } = true;
        
        // config to sound good
        private readonly float mBackgroundMusicVolume = 0.7f;

        internal SoundManager(int maxSoundEffectInstances = 5)
        {
            SoundEffects = new Hashtable();
            SoundEffectInstances = new Hashtable();
            MaxSoundEffectInstances = maxSoundEffectInstances;
        }

        public void LoadContent(ContentManager contentManager, List<string> soundNamesList)
        {
            CreateSoundEffects(contentManager, soundNamesList);
            CreateSoundEffectInstances();
        }

        private void CreateSoundEffects(ContentManager contentManager, List<string> soundNamesList)
        { 
            foreach (var soundFile in soundNamesList)
            {
                if (SoundEffects[soundFile] != null)
                {
                    SoundEffects.Remove(soundFile);
                }

                var soundFilePath = Path.Combine("SoundEffects", soundFile);
                var soundEffect = contentManager.Load<SoundEffect>(soundFilePath);
                SoundEffects.Add(soundFile, soundEffect);
            }
        }

        private void CreateSoundEffectInstances()
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
            volume = volume <= 1 ? volume  : 1;
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

                    if (sliderValue >= 0 && sliderValue <=1)
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

