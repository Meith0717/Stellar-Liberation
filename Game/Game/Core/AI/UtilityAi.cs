﻿// UtilityAi.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.AI
{
    public class UtilityAi
    {
        private Behavior mCurrentBehavior;
        private Behavior mLastBehavior;
        private readonly  HashSet<Behavior> mBehaviors;
        public bool Debug = false;

        private int mCoolDown;

        public UtilityAi(HashSet<Behavior> behaviors)
        {
            mBehaviors = behaviors;
            mCoolDown = ExtendetRandom.Random.Next(500);
        }

        public void AddBehavior(Behavior behavior)
        {
            mBehaviors.Add(behavior);
        }

        public void Update(GameTime gameTime, SpaceShip spaceShip, Scene scene)
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

        private Behavior SelectBehavior(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            PriorityQueue<Behavior, double> behaviors = new();
            foreach (var item in mBehaviors)
            {
                var score = item.GetScore(gameTime, spaceShip, scene);
                System.Diagnostics.Debug.WriteLineIf(Debug, $"{item.GetType().Name}: {Math.Round(score, 5)}");
                behaviors.Enqueue(item, -score);
            }
            if (!behaviors.TryPeek(out var behavior, out var priority)) return null;
            System.Diagnostics.Debug.WriteLineIf(Debug, $"_____________");
            return behavior;
        }
    }
}

