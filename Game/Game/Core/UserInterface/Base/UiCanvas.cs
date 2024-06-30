// UiFrame.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.UserInterface
{
    public class UiCanvas : UiElement
    {
        private readonly LinkedList<UiElement> mChildren = new();
        private readonly Dictionary<object, UiElement> mKeyValuePairs = new();
        public float Alpha = .5f;
        

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public void AddChild(object ID, UiElement child)
        {
            mChildren.AddLast(child);
            mKeyValuePairs.Add("", child);
        }

        public UiElement GetChild(object key) => mKeyValuePairs[key];

        public void ClearChilds() => mChildren.Clear();

        public bool AnyChild() => mChildren.Any();

        public override void Update(InputState inputState, GameTime gameTime)
        {
            base.Update(inputState, gameTime);
            foreach (var child in mChildren)
                child.Update(inputState, gameTime);
        }

        public override void ApplyResolution(Rectangle root, Resolution resolution)
        {
            base.ApplyResolution(root, resolution);
            foreach (var child in mChildren)
                child.ApplyResolution(Bounds, resolution);
        }

        public override void Draw()
        {
            TextureManager.Instance.SpriteBatch.FillRectangle(Bounds, Color.Black * Alpha, 1);
            foreach (var child in mChildren) child.Draw();
            DrawCanvas();
        }
    }
}

