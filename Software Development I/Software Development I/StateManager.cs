using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Software_Development_I
{
    class StateManager
    {
        private const int RUNNING = 1;
        private const int PAUSED = 2;
        private int state;

        public ContentManager contentMain;
        public ContentManager contentTemp;
        public SpriteBatch spriteBatch;

        //Meta-level game state
        private const int numberOfLevels = 1;
        private int levelIndex = -1;
        private Level level;
        private bool continuePressed;

        public StateManager(Game game)
        {
            state = 1;
        } //end StateManager

        public void LoadNextLevel()
        {
            //Set the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            if (level != null)
                level.Dispose();

            //Load the level
            string levelPath = string.Format("Content/Levels/{0}.map", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(fileStream, levelIndex);

        } //end LoadNextLevel()

        public void ReloadLevel()
        {
            levelIndex--;
            LoadNextLevel();
        } //end ReloadLevel

        public void Update(GameTime gameTime, KeyboardState keyboard, GamePadState gamePad)
        {
            level.Update(
        } //end Update

    } //end StateManager
} //end
