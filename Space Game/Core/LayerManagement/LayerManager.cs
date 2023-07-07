using GalaxyExplovive.Core.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyExplovive.Core.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new LinkedList<Layer>();

    public LayerManager(Game1 game)
    {
        mGame = game;
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        mLayerStack.AddLast(layer);
    }

    public void PopLayer()
    {
        if (mLayerStack.Last != null)
        {
            mLayerStack.Last.Value.Destroy();
            mLayerStack.RemoveLast();
        }
    }

    // update layers
    public void Update(GameTime gameTime, InputState inputState)
    {
        foreach (Layer layer in mLayerStack.Reverse())
        {
            layer.Update(gameTime, inputState);
            if (!layer.UpdateBelow)
            {
                break;
            }
        }
    }

    // draw layers
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Layer layer in mLayerStack)
        {
            layer.Draw(spriteBatch);
        }
    }

    // lifecycle methods
    public void Exit()
    {
        foreach (Layer layer in mLayerStack)
        {
            layer.Destroy();
        }
        mGame.Exit();
    }

    // fullscreen stuff
    public void OnResolutionChanged()
    {
        foreach (Layer layer in mLayerStack.ToArray())
        {
            layer.OnResolutionChanged();
        }
    }

    public bool ContainsLayer(Layer layer)
    {
        return mLayerStack.Contains(layer);
    }
}