// UiLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.UserInterface
{
    internal class UiLayer : UiElement
    {
        public Color Color = Color.White;
        public double Alpha = 1;
        private LinkedList<UiElement> mChildren = new LinkedList<UiElement>();
        private Texture2D mTexture;

        public UiLayer()
            : base()
        {
            mTexture = TextureManager.Instance.GetTexture(TextureRegistries.layer);
        }


        public override void Initialize(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.Initialize(mCanvas.Bounds);
        }

        public void AddChild(UiElement child) => mChildren.AddLast(child);

        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            TextureManager.Instance.SpriteBatch.Draw(mTexture, mCanvas.Bounds, color);
            foreach (var child in mChildren) child.Draw();
            mCanvas.Draw();
        }

        public override void Update(InputState inputState, Rectangle root)
        {
            foreach (var child in mChildren) child.Update(inputState, mCanvas.Bounds);
        }

        public override void OnResolutionChanged(Rectangle root)
        {
            mCanvas.UpdateFrame(root);
            foreach (var child in mChildren) child.OnResolutionChanged(mCanvas.Bounds);
        }
    }
}
