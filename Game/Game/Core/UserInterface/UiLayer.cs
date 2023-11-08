// UiLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiLayer : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private LinkedList<UiElement> mChildren = new LinkedList<UiElement>();

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            var texture = TextureManager.Instance.GetTexture(ContentRegistry.layer);
            TextureManager.Instance.SpriteBatch.Draw(texture, Frame, color);
            foreach ( var child in mChildren ) child.Draw();
        }

        public override void Update(InputState inputState, Rectangle root)  
        {
            foreach (var child in mChildren) child.Update(inputState, Frame);
        }

        public override void Initialize(Rectangle root)
        {
            base.Initialize(root);
            foreach (var child in mChildren) child.Initialize(Frame);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            foreach (var child in mChildren) child.OnResolutionChanged(Frame);
            base.OnResolutionChanged(root);
        }
    }
}
