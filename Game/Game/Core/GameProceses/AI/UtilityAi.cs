﻿// UtilityAi.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
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

        public void Update(GameTime gameTime)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            // if (mCoolDown > 0) return;
            // mCoolDown = 500;

            // Auswahl des Verhaltens basierend auf Prioritäten und Spielsituation
            mCurrentBehavior = SelectBehavior();
            if (mCurrentBehavior != mLastBehavior)
                mLastBehavior?.Recet();
            mLastBehavior = mCurrentBehavior;

            // Ausführung des ausgewählten Verhaltens
            mCurrentBehavior?.Execute();
        }

        private Behavior SelectBehavior()
        {
            DebugMessage = "";
            PriorityQueue<Behavior, double> behaviors = new();
            foreach (var item in mBehaviors)
            {
                var score = item.GetScore();
                DebugMessage += $"{item.GetType().Name}: {Math.Round(score, 5)}\n";
                behaviors.Enqueue(item, -score);
            }
            if (!behaviors.TryPeek(out var behavior, out var _)) return null;
            return behavior;
        }
    }
}

