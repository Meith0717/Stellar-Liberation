using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    internal class UiLayer : UiElement
    {
        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;
        public Color Color = Color.White;
        public float Alpha = 1f;
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;

        private List<UiElement> childs = new List<UiElement>();

        public UiLayer(UiLayer root, float relX, float relY, float relWidth, float relHeight) 
        {
            Canvas = new(root, relX, relY, relWidth, relHeight);
            if (root != null )
            {
                root.Addchild(this);
            }
        }
        public override void Draw()
        {
            var color = new Color((int)(Color.R * Alpha), (int)(Color.G * Alpha), (int)(Color.B * Alpha), (int)(Color.A * Alpha));
            TextureManager tm = TextureManager.Instance;
            tm.GetSpriteBatch().Draw(tm.GetTexture("UiLayer"), Canvas.ToRectangle(), color);
            foreach (UiElement child in childs)
            {
                child.Draw();
            }
        }

        public override void OnResolutionChanged()
        {
            Canvas.Fill = Fill;
            Canvas.Side = Side;
            if (MaxWidth > 0) { Canvas.MaxWidth = MaxWidth; }
            if (MinWidth > 0) { Canvas.MinWidth = MinWidth; }
            if (MaxHeight > 0) { Canvas.MaxHeight = MaxHeight; }
            if (MinHeight > 0) { Canvas.MinHeight = MinHeight; }
            Canvas.Update();
            foreach (UiElement child in childs)
            {
                child.OnResolutionChanged();
            }

        }

        public override void Update(InputState inputState)
        {
            foreach (UiElement child in childs)
            {
                child.Update(inputState);
            }

        }

        protected void Addchild(UiElement child)
        {
            childs.Add(child);
        }
    }
}
