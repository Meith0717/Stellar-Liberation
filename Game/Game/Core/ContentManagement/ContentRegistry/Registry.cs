// Registry.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System.IO;

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class Registry
    {
        public string Name;
        public string FilePath;

        public Registry(string dirPath, string fileName)
        {
            FilePath = Path.Combine(dirPath, fileName);
            Name = fileName;
        }

        public static implicit operator string(Registry registry) => registry.Name;
    }
}
