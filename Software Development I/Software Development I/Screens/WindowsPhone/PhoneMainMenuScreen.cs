#region File Description
//-----------------------------------------------------------------------------
// PhoneMainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Software_Development_I;
using Microsoft.Xna.Framework;

namespace Software_Development_I
{
    class PhoneMainMenuScreen : PhoneMenuScreen
    {
        public PhoneMainMenuScreen()
            : base("Main Menu")
        {
            // Create a button to start the game
            Button playButton = new Button("Play");
            playButton.Tapped += playButton_Tapped;
            MenuButtons.Add(playButton);

            // Create two buttons to toggle sound effects and music. This sample just shows one way
            // of making and using these buttons; it doesn't actually have sound effects or music
            Button levelButton = new Button("Level Select: " + GameOptions.levelSelected);
            levelButton.Tapped += levelButton_Tapped;
            MenuButtons.Add(levelButton);

            Button controlButton = new Button("Controls: Touch");
            controlButton.Tapped += controlButton_Tapped;
            MenuButtons.Add(controlButton);
        }

        void playButton_Tapped(object sender, EventArgs e)
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }

        void levelButton_Tapped(object sender, EventArgs e)
        {
            GameOptions.levelSelected++;

            if (GameOptions.levelSelected > 2)
                GameOptions.levelSelected = 1;

            Button button = sender as Button;
            button.Text = "Level Select: " + GameOptions.levelSelected;
        }

        void controlButton_Tapped(object sender, EventArgs e)
        {
            GameOptions.controlOptions++;

            if (GameOptions.controlOptions > 3)
                GameOptions.controlOptions = 1;

            Button button = sender as Button;

            if (GameOptions.controlOptions == 1)
                button.Text = "Controls: Touch";
            if (GameOptions.controlOptions == 2)
                button.Text = "Controls: Motion";
            if (GameOptions.controlOptions == 3)
                button.Text = "Controls: Both";

        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
            base.OnCancel();
        }

        public static class GameOptions
        {
            public static int levelSelected = 1;
            public static int controlOptions = 1;
        }
    }
}
