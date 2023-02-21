using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.Menu;
using Space_Game.Core;
using Space_Game.Core.LayerManagement;

namespace rache_der_reti.Game.Layers;

public class HudLayer : Layer
{
    private const int BottomBarHeight = 60;
    private const int IconSize = 40;

    private UiElement mRoot;
    private UiElementText mTimer;

    // message pushing
    private UiElementText mScreenMessage;
    private readonly List<(string, string)> mScreenMessagesLog;
    private int mScreenMessageMillisLeft;
    private bool mScreenMessageCurrentlyShown;

    public HudLayer()
    {
        mScreenMessagesLog = new List<(string, string)>();
        UpdateBelow = true;
        Initialize();
    }

    private void Initialize()
    {
        // root element
        mRoot = new UiElement();

        // bottom bar
        UiElement bottomBar = new UiElement();
        mRoot.ChildElements.Add(bottomBar);
        bottomBar.BackgroundColor = new Color(0, 0, 0);
        bottomBar.BackgroundAlpha = 1f;
        bottomBar.Height = BottomBarHeight;
        bottomBar.WidthPercent = 100;
        bottomBar.MyHorizontalAlignt = UiElement.HorizontalAlignment.Center;
        bottomBar.MyVerticalAlignment = UiElement.VerticalAlignment.Top;

        // screen message
        mScreenMessage = new UiElementText("", "text");
        mScreenMessage.BackgroundAlpha = 0.05f; // changed from 0.01.
        mScreenMessage.MyVerticalAlignment = UiElement.VerticalAlignment.Bottom;
        mScreenMessage.SetMargin(-200, -10);
        mScreenMessage.setOnClickPointer(RemoveMessage);
        mRoot.ChildElements.Add(mScreenMessage);
        RemoveMessage();

        // bar left part
        UiElement el1 = new UiElementList(false);
        el1.MyHorizontalAlignt = UiElement.HorizontalAlignment.Left;
        bottomBar.ChildElements.Add(el1);

        // bar center part
        UiElement el2 = new UiElementList(false);
        el2.MyHorizontalAlignt = UiElement.HorizontalAlignment.Center;
        bottomBar.ChildElements.Add(el2);

        // right part of bar
        UiElement el5 = new UiElementList(false);
        el5.MyHorizontalAlignt = UiElement.HorizontalAlignment.Right;
        bottomBar.ChildElements.Add(el5);

        // timer
        mTimer = new UiElementText("", "text");
        el5.ChildElements.Add(mTimer);
        mTimer.DimensionParts = 4;
        mTimer.FontColor = Color.White;

        OnResolutionChanged();
    }

    public override void Update(GameTime gameTime, InputState inputState)
    {
        mRoot.HandleInput(inputState);

        // set timer
        int totalSeconds = (int)Math.Round(Globals.mGameLayer.mPassedSeconds);
        string minutes = (totalSeconds / 60).ToString();
        string seconds = "0" + (totalSeconds % 60);
        seconds = seconds[new Range(seconds.Length - 2, seconds.Length)];
        mTimer.UpdateText(minutes + ":" + seconds);

        // set screen message
        mScreenMessageMillisLeft -= gameTime.ElapsedGameTime.Milliseconds;
        if (mScreenMessageMillisLeft < 0)
        {
            RemoveMessage();
        }
        OnResolutionChanged();
    }

    public override void Draw()
    {
        mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        mRoot.Render();
        mSpriteBatch.End();
    }

    private string AddLineBreaks(string message, int maxLength=60)
    {
        int stringLength = message.Length;
        if (stringLength > maxLength)
        {
            // find location of next space.
            int idxNextSpace = message.Substring(maxLength, stringLength - maxLength).IndexOf(" ", StringComparison.Ordinal);

            if (idxNextSpace != 0 & idxNextSpace > 0)
            {
                int idxBreak = idxNextSpace + maxLength;
                message = message.Substring(0, idxBreak) + "\n" + message.Substring(idxBreak + 1, stringLength - idxBreak - 1);
            }
        }
        return message;
    }

    public void PushMessage(string message, int seconds, bool saveMsg = true, bool lineBreak = true)
    {
        // add line breaks
        if (lineBreak)
        {
            message = AddLineBreaks(message);
        }
        
        // if a message is already shown and a new message shows up, show both:
        var messageOut = message;
        
        if (mScreenMessageCurrentlyShown)
        {
            // Add previous message to text if both are different.
            if (mScreenMessagesLog.Count >= 1)
            {
                // ... and if new message is not too long (usually happens if message is log).
                if (message.Length < 150)
                {
                    messageOut =  message + "\n \n" + mScreenMessagesLog.Last().Item1;
                }
            }
        }

        mScreenMessageMillisLeft = seconds * 1000;
        mScreenMessage.UpdateText(messageOut);
        mScreenMessage.FontColor = Color.White;
        mScreenMessage.HoverBackgroundColor = Color.Gray;
        mScreenMessageCurrentlyShown = true;

        // Save msg in log with time stamp.
        if (saveMsg)
        {
            var currentTime = mTimer.mText;

            if (mScreenMessagesLog.Count >= 1)
            {
                if (message != mScreenMessagesLog.Last().Item1)
                {
                    mScreenMessagesLog.Add((message, currentTime));
                }
            }
            else
            {
                mScreenMessagesLog.Add((message, currentTime));
            }
        }
    }

    private void RemoveMessage()
    {
        mScreenMessageCurrentlyShown = false;
        mScreenMessage.FontColor = Color.Transparent;
        mScreenMessage.BackgroundColor = Color.Transparent;
    }

    public override void Destroy()
    {
        // change statistics game time
        // mPersistence.MyStatistics.TotalGameTimeInSeconds += (int)Globals.mGameLayer.mPassedSeconds;
        // mPersistence.Save();
    }

    public override void OnResolutionChanged()
    {
        mRoot.Update(new Rectangle(0,0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
    }
}