// UtilityAi.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.AI
{
    public class UtilityAi
    {
        private Behavior mCurrentBehavior;
        private Behavior mLastBehavior;
        private readonly List<Behavior> mBehaviors;
        public bool Debug = false;
        public string DebugMessage { get; private set; } = "";

        private int mCoolDown;

        public UtilityAi(List<Behavior> behaviors)
        {
            mCoolDown = ExtendetRandom.Random.Next(500);
            mBehaviors = behaviors;
        }

        public UtilityAi()
        {
            mCoolDown = ExtendetRandom.Random.Next(500);
            mBehaviors = new();
        }

        public void AddBehavior(Behavior behavior)
        {
            mBehaviors.Add(behavior);
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = 500;

            // Auswahl des Verhaltens basierend auf Prioritäten und Spielsituation
            mCurrentBehavior = SelectBehavior(gameTime, spaceShip, scene);

            if (mCurrentBehavior != mLastBehavior) mLastBehavior?.Reset(spaceShip);

            // Ausführung des ausgewählten Verhaltens
            mCurrentBehavior?.Execute(gameTime, spaceShip, scene);
            mLastBehavior = mCurrentBehavior;
        }

        private Behavior SelectBehavior(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            DebugMessage = "";
            PriorityQueue<Behavior, double> behaviors = new();
            foreach (var item in mBehaviors)
            {
                var score = item.GetScore(gameTime, spaceShip, scene);
                DebugMessage += $"{item.GetType().Name}: {Math.Round(score, 5)}\n";
                behaviors.Enqueue(item, -score);
            }
            if (!behaviors.TryPeek(out var behavior, out var _)) return null;
            return behavior;
        }
    }
}

