// GameLayerFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Layers;
using System.IO;

namespace StellarLiberation.Game.Core.CoreProceses
{
    public class GameLayerFactory
    {
        private readonly Serializer mSerializer;
        private readonly LayerManager mLayerManager;
        private GameLayer mGameLayer;
        public readonly string GameSaveDirectory = "gameSave";

        public string GameSaveFilePath => Path.Combine(GameSaveDirectory, "save");

        public GameLayerFactory(Serializer serializer, LayerManager layerManager) 
        {
            mSerializer = serializer;
            mLayerManager = layerManager;
            mSerializer.CreateFolder(GameSaveDirectory);
        }

        public void NewGameLayer()
        {
            if (mSerializer.FileExist(GameSaveFilePath)) System.Diagnostics.Debug.WriteLine("Game will be override");
            mGameLayer = new();
            mLayerManager.AddLayer(mGameLayer);
        }

        public void LoadGameLayer()
        {
            if (!mSerializer.FileExist(GameSaveFilePath)) throw new FileNotFoundException();
            mGameLayer = new();
            mGameLayer = (GameLayer)mSerializer.PopulateObject(mGameLayer, GameSaveFilePath);
            mLayerManager.AddLayer(mGameLayer);
        }

        public void SaveGameLayer()
        {
            if (mGameLayer is null) throw new System.Exception();
            mSerializer.SerializeObject(mGameLayer, GameSaveFilePath);
        }
    }
}
