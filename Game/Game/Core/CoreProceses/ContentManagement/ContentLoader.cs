// ContentLoader.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using static System.Net.WebRequestMethods;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{
    public class ContentLoader
    {
        private readonly ContentManager mContent;
        private readonly string[] mMainDirrectories;

        public string ProcessMessage { get; private set; } = "";
        public double Process { get; private set; } = 0;

        internal ContentLoader(ContentManager content)
        {
            mContent = content;
            mMainDirrectories = GetDirectorys(content.RootDirectory);
        }

        private string[] GetFiles(string directoryPath) => Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

        private string[] GetDirectorys(string contentPath) => Directory.GetDirectories(contentPath, "*.*", SearchOption.TopDirectoryOnly);

        public void LoadEssenzialContent()
        {
            TextureManager.Instance.LoadBuildTextureContent(mContent, "missingContent", "missingContent");
            LoadBuildContent("fonts", TextureManager.Instance.LoadBuildFontContent);
            LoadBuildContent("gui", TextureManager.Instance.LoadBuildTextureContent);
        }

        public void LoadContentAsync(Action onLoadComplete, Action<Exception> onError)
        {
            Thread thread = new(() => {
                try
                {
                    LoadBuildContent("music", MusicManager.Instance.LoadBuildContent);
                    LoadBuildContent("sfx", SoundEffectManager.Instance.LoadBuildContent);
                    LoadBuildContent("textures", TextureManager.Instance.LoadBuildTextureContent);
                    LoadWeapons();
                    ProcessMessage = "Ready";
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }
                onLoadComplete.Invoke();
            });

            thread.Start();
        }

        private void LoadBuildContent(string contentDirectory, Action<ContentManager, string, string> managerLoader)
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, contentDirectory);
            var files = GetFiles(rootDirectory);
            Process = 0;

            for (int i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var split = filePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
                if (split.Count > 1) split.RemoveAt(0);
                var directory = Path.GetDirectoryName(Path.Combine(split.ToArray()));
                var pathWithoutExtension = Path.Combine(directory, fileName);
                ProcessMessage = $"Loading: {filePath}";
                Process += 1d / files.Length;
                managerLoader.Invoke(mContent, fileName, pathWithoutExtension);
            }
        }

        private Dictionary<string, JsonElement> LoadJsonDictionary(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            string jsonString = reader.ReadToEnd();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString, options);
        }

        private void LoadWeapons()
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, "weapons");
            var weaponsDirectorys = GetDirectorys(rootDirectory);
            Process = 0;
            foreach (var weaponDirectory in weaponsDirectorys)
            {
                var weaponID = Path.GetFileName(weaponDirectory);
                var weaponFiles = GetFiles(weaponDirectory);

                ProcessMessage = $"Loading: {weaponFiles}";
                Process += 1d / weaponsDirectorys.Length;

                foreach (string weaponFile in weaponFiles)
                {
                    var fileName = Path.GetFileName(weaponFile);

                    switch (fileName.ToLower())
                    {
                        case "config.json":
                            var jsonDict = LoadJsonDictionary(weaponFile);
                            WeaponFactory.Instance.AddConfig(weaponID, jsonDict);
                            break;
                        case "obj.png":
                            TextureManager.Instance.LoadTextureContent($"{weaponID}Obj", weaponFile);
                            break;
                        case "proj.png":
                            TextureManager.Instance.LoadTextureContent($"{weaponID}Proj", weaponFile);
                            break;
                    }
                }
            }
        }
    }
}
