using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using static Galaxy_Explovive.Core.UserInterface.UiCanvas;

namespace Galaxy_Explovive.Core.UserInterface.Widgets
{
    public class UiFrame : UiElement
    {
        private List<UiElement> mChilds = new();

        public RootFill Fill = RootFill.Fix;
        public RootSide Side = RootSide.None;
        public float RelativX = 0.5f;
        public float RelativY = 0.5f;
        public float RelativeH = 0.5f;
        public float RelativeW = 0.5f;
        public float? Height = null;
        public float? Width = null;
        public Color Color = Color.White;
        public float Alpha = 1f;
        public int EdgeWidth = 0;
        public int MarginX = 0;
        public int MarginY = 0;
        public bool Hide = false;

        public UiFrame(UiFrame root, TextureManager textureManager, GraphicsDevice graphicsDevice) 
            : base(root, textureManager, graphicsDevice) { }

        public override void Draw()
        {
            DrawLayer(EdgeWidth);
            foreach (UiElement child in mChilds)
            {
                child.Draw();
            }
        }

        public override void OnResolutionChanged()
        {
            if (Hide) return;
            mCanvas.RelativeX = RelativX;
            mCanvas.RelativeY = RelativY;
            mCanvas.RelativeW = RelativeW;
            mCanvas.RelativeH = RelativeH;
            mCanvas.Width = Width; 
            mCanvas.Height = Height;
            mCanvas.Fill = Fill;
            mCanvas.Side = Side;
            mCanvas.MarginX = MarginX;
            mCanvas.MarginY = MarginY;
            mCanvas.OnResolutionChanged(mGraphicsDevice);
            foreach(UiElement child in mChilds)
            {
                child.OnResolutionChanged();
            }
        }

        public override void Update(InputState inputState)
        {
            if (Hide) return;
            foreach (UiElement child in mChilds)
            {
                child.Update(inputState);
            }

        }

        public void AddToChilds(UiElement child)
        {
            mChilds.Add(child);
        }

        private void DrawLayer(int EdgeWidth)
        {
            if (Hide) return;
            Color color = new(
                (int)(Color.R * Alpha),
                (int)(Color.G * Alpha),
                (int)(Color.B * Alpha),
                (int)(Color.A * Alpha)
                );

            TextureManager tm = mTextureManager;
            Texture2D rectangle = tm.GetTexture("Layer");
            Texture2D edge = tm.GetTexture("Circle");
            Rectangle canvas = mCanvas.ToRectangle();

            // Draw 4 Edges
            canvas.X += EdgeWidth / 2; canvas.Y += EdgeWidth / 2;
            canvas.Width -= EdgeWidth; canvas.Height -= EdgeWidth;
            foreach (Point point in canvas.GetCorners())
            {
                Point edgePoint = point - new Point(EdgeWidth / 2, EdgeWidth / 2);
                tm.SpriteBatch.Draw(edge, new Rectangle(edgePoint, new Point(EdgeWidth, EdgeWidth)), color);
            }

            // Draw 4 Borgers
            for (int i = 0; i < 3; i++)
            {
                tm.SpriteBatch.DrawLine(canvas.GetCorners()[i].ToVector2(), canvas.GetCorners()[i + 1].ToVector2(), color, EdgeWidth, 1);
            }
            tm.SpriteBatch.DrawLine(canvas.GetCorners()[3].ToVector2(), canvas.GetCorners()[0].ToVector2(), color, EdgeWidth, 1);

            // raw inner Rectangle
            tm.SpriteBatch.Draw(rectangle, canvas, color);
        }
    }
}
