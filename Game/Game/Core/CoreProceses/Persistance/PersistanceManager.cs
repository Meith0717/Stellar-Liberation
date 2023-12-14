// GameLayerFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Layers;
using System.IO;

namespace StellarLiberation.Game.Core.CoreProceses.Persistance
{
    public class PersistanceManager
    {
        private readonly Serializer mSerializer;
        public readonly string GameSaveDirectory = "gameSave";

        public string GameSaveFilePath => Path.Combine(GameSaveDirectory, "save");

        public PersistanceManager()
        {
            mSerializer = new(".stellarLieberation");
            mSerializer.CreateFolder(GameSaveDirectory);
        }

        public GameLayer LoadGameLayer()
        {
            if (!mSerializer.FileExist(GameSaveFilePath)) throw new FileNotFoundException();
            var gameLayer = new GameLayer();
            return (GameLayer)mSerializer.PopulateObject(gameLayer, GameSaveFilePath);
        }

        public void SaveGameLayer(GameLayer gameLayer)
        {
            if (gameLayer is null) throw new System.Exception();
            mSerializer.SerializeObject(gameLayer, GameSaveFilePath);
        }
    }
}
