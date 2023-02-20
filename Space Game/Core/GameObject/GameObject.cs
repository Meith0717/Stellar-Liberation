using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.TextureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Game.Core.GameObject
{
    public abstract class GameObject
    {
        // Location
        [JsonProperty] public Vector2 Position;
        [JsonProperty] public Vector2 Offset;
        // Rendering
        [JsonProperty] public string TextureId;
        [JsonProperty] public int TextureWidth;
        [JsonProperty] public int TextureHeight;
        // Hover Stuff
        [JsonProperty] public bool Hover;
        [JsonProperty] public CircleF HoverBox;
        [JsonProperty] public string NormalTextureId;
        [JsonProperty] public string HoverTectureId;

        // Update Logic
        public abstract void Update(GameTime gameTime, InputState inputState);
        public void ManageHover(InputState inputState, Action? OnClicked = null, Action? OnDoubleClicked = null)
        {
            Vector2 mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            if (HoverBox.Contains(mousePosition))
            {
                Hover = true;
                if (inputState.mMouseActionType == MouseActionType.LeftClick)
                {
                    if (OnClicked == null) { return; }
                    OnClicked();
                    return;
                }
                if (inputState.mMouseActionType == MouseActionType.LeftClickDouble)
                {
                    if (OnDoubleClicked == null) { return; }
                    OnDoubleClicked();
                    return;
                }
                return;
            }
            Hover = false;
        }

        // Draw Logic
        public abstract void Draw();
    }
}
