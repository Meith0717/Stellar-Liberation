using Galaxy_Explovive.Core.TextureManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.UserInterface
{
    internal abstract class MyUiElement
    {
        public abstract void Update();
        public abstract void OnResolutionChanged();
        public abstract void Draw(TextureManager textureManager);
    }
}
