// MyUiElement.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace CelestialOdyssey.Game.Core.UserInterface
{
    internal abstract class MyUiElement
    {
        public abstract void Update();
        public abstract void OnResolutionChanged();
        public abstract void Draw();
    }
}
