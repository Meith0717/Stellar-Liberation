using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.ComponentModel;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiLayer : UiElement
    {
        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;
        public Color Color = Color.White;
        public float Alpha = 1f;
        public int MinWidth;
        public int MaxWidth;
        public int MinHeight;
        public int MaxHeight;
        public int Borgerwidth = 0;

        private List<UiElement> childs = new List<UiElement>();

        public UiLayer(UiLayer root, float relX, float relY, float relWidth, float relHeight) : base(root) 
        {
            Canvas = new(root, relX, relY, relWidth, relHeight);
        }
        public override void Draw()
        {
            DrawLayer(Borgerwidth);
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

        public void Addchild(UiElement child)
        {
            childs.Add(child);
        }

        private void DrawLayer(int borgerWidth)
        {
            Color color = new Color(
                (int)(Color.R * Alpha),
                (int)(Color.G * Alpha),
                (int)(Color.B * Alpha),
                (int)(Color.A * Alpha)
                );

            TextureManager tm = TextureManager.Instance;
            Texture2D rectangle = tm.GetTexture("Layer");
            Texture2D edge = tm.GetTexture("Circle");
            Rectangle canvas = Canvas.ToRectangle();

            // Draw 4 Edges
            canvas.X += borgerWidth / 2; canvas.Y += borgerWidth / 2;
            canvas.Width -= borgerWidth; canvas.Height -= borgerWidth;
            foreach (Point point in canvas.GetCorners())
            {
                Point edgePoint = point - new Point(borgerWidth/2, borgerWidth/2); 
                tm.GetSpriteBatch().Draw(edge, new Rectangle(edgePoint, new Point(borgerWidth, borgerWidth)), color);
            }

            // Draw 4 Borgers
            for (int i = 0; i < 3; i++) 
            {            
                tm.GetSpriteBatch().DrawLine(canvas.GetCorners()[i].ToVector2(), canvas.GetCorners()[i + 1].ToVector2(), color, borgerWidth, 1);
            }
            tm.GetSpriteBatch().DrawLine(canvas.GetCorners()[3].ToVector2(), canvas.GetCorners()[0].ToVector2(), color, borgerWidth, 1);
            
            // raw inner Rectangle
            tm.GetSpriteBatch().Draw(rectangle, canvas, color);
        }
    }
}
