// ContentLoader.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.GameObjects.SpaceCrafts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

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
            EffectManager.Instance.LoadBuildContent(mContent, "Blur");
            TextureManager.Instance.LoadBuildTextureContent(mContent, "missingContent", "missingContent");
            LoadBuildContent("fonts", TextureManager.Instance.LoadBuildFontContent);
            LoadBuildContent("gui", TextureManager.Instance.LoadBuildTextureContent);
        }

        public void LoadContentAsync(Action onLoadComplete, Action<Exception> onError)
        {
            Thread thread = new(() =>
            {
                try
                {
                    LoadBuildContent("music", MusicManager.Instance.LoadBuildContent);
                    LoadBuildContent("sfx", SoundEffectManager.Instance.LoadBuildContent);
                    LoadBuildContent("textures", TextureManager.Instance.LoadBuildTextureContent);
                    LoadWeapons();
                    LoadBattleShips();
                    LoadFlagShips();
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
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString, options);
            return dict;
        }

        private void LoadWeapons()
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, "weapons");
            var weaponsDirectorys = GetDirectorys(rootDirectory);
            Process = 0;
            foreach (var weaponDirectory in weaponsDirectorys)
            {
                ProcessMessage = $"Loading: {weaponDirectory}";
                Process += 1d / weaponsDirectorys.Length;
                var weaponID = Path.GetFileName(weaponDirectory);
                WeaponFactory.Instance.AddConfig(weaponID, LoadJsonDictionary(Path.Combine(weaponDirectory, "config.json")));
                TextureManager.Instance.LoadTextureContent($"{weaponID}Obj", Path.Combine(weaponDirectory, "obj.png"));
                TextureManager.Instance.LoadTextureContent($"{weaponID}Proj", Path.Combine(weaponDirectory, "proj.png"));
            }
        }

        private void LoadBattleShips()
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, "battleships");
            var shipsDirectorys = GetDirectorys(rootDirectory);
            Process = 0;
            foreach (var shipDirectory in shipsDirectorys)
            {
                ProcessMessage = $"Loading: {shipDirectory}";
                Process += 1d / shipsDirectorys.Length;
                var shipID = Path.GetFileName(shipDirectory);
                BattleshipFactory.Instance.AddConfig(shipID, LoadJsonDictionary(Path.Combine(shipDirectory, "config.json")));
                BattleshipFactory.Instance.AddWeapons(shipID, LoadJsonDictionary(Path.Combine(shipDirectory, "weapons.json")));
                TextureManager.Instance.LoadTextureContent($"{shipID}", Path.Combine(shipDirectory, "main.png"));
                TextureManager.Instance.LoadTextureContent($"{shipID}Shield", Path.Combine(shipDirectory, "shield.png"));
            }
        }

        private void LoadFlagShips()
        {
            var rootDirectory = Path.Combine(mContent.RootDirectory, "flagships");
            var shipsDirectorys = GetDirectorys(rootDirectory);
            Process = 0;
            foreach (var shipDirectory in shipsDirectorys)
            {
                ProcessMessage = $"Loading: {shipDirectory}";
                Process += 1d / shipsDirectorys.Length;
                var shipID = Path.GetFileName(shipDirectory);
                FlagshipFactory.Instance.AddConfig(shipID, LoadJsonDictionary(Path.Combine(shipDirectory, "config.json")));
                FlagshipFactory.Instance.AddWeapons(shipID, LoadJsonDictionary(Path.Combine(shipDirectory, "weapons.json")));
                TextureManager.Instance.LoadTextureContent($"{shipID}", Path.Combine(shipDirectory, "main.png"));
                TextureManager.Instance.LoadTextureContent($"{shipID}Shield", Path.Combine(shipDirectory, "shield.png"));
            }
        }



    }
}
