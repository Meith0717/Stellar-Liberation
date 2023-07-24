using Galaxy_Explovive.Core.ActionManager.Actions;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game;
using Galaxy_Explovive.Game.GameObjects;
using Galaxy_Explovive.Game.GameObjects.Astronomical_Body;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using System;

namespace Galaxy_Explovive.Core.ActionManager
{
    public class ActionManager
    {
        private readonly GameLayer mGameLayer;
        public ActionManager(GameLayer gameLayer) { mGameLayer = gameLayer; }

        public void Update(SelectableObject hoveredObject, InputState inputState) 
        {
            if (hoveredObject == null) { return; }

            Type selectType = hoveredObject.GetType();

            if (selectType == typeof(Star))
            {
                StarActions.Update((Star)hoveredObject, mGameLayer, inputState);
            }
            if (selectType == typeof(Planet))
            {
                PlanetActions.Update((Planet)hoveredObject, mGameLayer, inputState);
            }
            if (selectType == typeof(Cargo))
            {
                mGameLayer.mCamera.TargetPosition = hoveredObject.Position;
            }
        }
    }
}
