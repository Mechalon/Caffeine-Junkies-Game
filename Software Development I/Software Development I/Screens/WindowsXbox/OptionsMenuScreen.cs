#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace Software_Development_I
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry levelSelectEntry;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Level Select")
        {
            // Create our menu entries.
            levelSelectEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            levelSelectEntry.Selected += levelSelectEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(levelSelectEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            levelSelectEntry.Text = "Select level: " + GameOptions.levelSelected;
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Level select menu entry is selected.
        /// </summary>
        void levelSelectEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameOptions.levelSelected++;

            if (GameOptions.levelSelected > 2)
                GameOptions.levelSelected = 1;
            SetMenuEntryText();
        }

        #endregion

        public static class GameOptions
        {
            public static int levelSelected = 1;
        }
    }
}
