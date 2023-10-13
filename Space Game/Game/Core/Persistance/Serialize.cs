// Serialize.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CelestialOdyssey.Game.Core.Persistance
{
    public class Serialize
    {
        private readonly string mSaveFilePath;

        public Serialize(string gameName = ".galaxy-explovive")
        {
            string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string GameDataPath = Path.Combine(AppDataPath, gameName);
            mSaveFilePath = Path.Combine(GameDataPath, "Saves");
            CreateFolder(GameDataPath);
            CreateFolder(mSaveFilePath);
        }

        private static void CreateFolder(string folderPath)
        {
            DirectoryInfo createFDirectoryInfo = new DirectoryInfo(folderPath);
            if (createFDirectoryInfo.Exists) return;
            createFDirectoryInfo.Create();
        }

        public void DeleteFile(string fileName)
        {
            var filePath = Path.Combine(mSaveFilePath, fileName + ".json");
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IO exception has occurred while attempting to delete the file:");
                Console.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("You do not have permission to delete the file:");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception has occurred while attempting to delete the file:");
                Console.WriteLine(ex.Message);
            }
        }

        public void SerializeObject(object obj, string fileName)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };

            var objectJson = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            File.WriteAllText(Path.Combine(mSaveFilePath, fileName + ".json"), objectJson);
        }

        public object PopulateObject(object obj, string fileName)
        {
            var filePath = fileName; // Path.Combine(mSaveFilePath, fileName + ".json");
            if (!File.Exists(filePath))
            {
                return null;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            using StreamReader r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            JsonConvert.PopulateObject(json, obj, jsonSerializerSettings);
            return obj;
        }

        public object DeserializeObject(Type objectType, string fileName)
        {
            var filePath = Path.Combine(mSaveFilePath, fileName + ".json");
            if (!File.Exists(filePath))
            {
                return null;
            }
            using var file = File.OpenText(filePath);
            JsonSerializer serializer = new JsonSerializer();
            var loadedObject = serializer.Deserialize(file, objectType);
            return loadedObject;
        }

        public T LoadJsonContent<T>(ContentManager content, object obj, string fileName)
        {
            var filePath = Path.Combine(Path.GetFullPath(content.RootDirectory), fileName);
            return (T)PopulateObject(obj, filePath);
        }
    }
}
