using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.SoundManagement;
using Space_Game;
using Space_Game.Game.Layers;

namespace rache_der_reti.Core.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame;
    private readonly GraphicsDevice mGraphicsDevice;
    private readonly SpriteBatch mSpriteBatch;
    private readonly ContentManager mContentManager;

    private readonly SoundManager mSoundManager;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new LinkedList<Layer>();

    public LayerManager(Game1 game, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        ContentManager contentManager, SoundManager soundManager)
    {
        mGame = game;
        mGraphicsDevice = graphicsDevice;
        mSpriteBatch = spriteBatch;
        mContentManager = contentManager;
        mSoundManager = soundManager;
        Start();
        //mGame.ToggleFullscreen();
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
    public void Update(GameTime gameTime, InputState inputState, GameWindow window, GraphicsDeviceManager graphic)
    {
        if (inputState.mActionList.Contains(ActionType.ToggleFullscreen))
        {
            mGame.ToggleFullscreen();
        }

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
    public void Draw()
    {
        foreach(Layer layer in mLayerStack)
        {
            layer.Draw();
        }
    }

    // lifecycle methods
    private void Start()
    {
         mLayerStack.AddLast(new GameLayer(this, mGraphicsDevice, mSpriteBatch, mContentManager, mSoundManager));
    }
    public void Exit()
    {
        foreach(Layer layer in mLayerStack)
        {
            layer.Destroy();
        }
        mGame.Exit();
    }

    // fullscreen stuff
    public void OnResolutionChanged()
    {
        foreach(Layer layer in mLayerStack.ToArray())
        {
            layer.OnResolutionChanged();
        }
    }
}