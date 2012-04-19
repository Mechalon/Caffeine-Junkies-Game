using System;
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
        private const int LOADING   = 1;
        private const int READY     = 2;
        private const int RUNNING   = 3;
        private const int PAUSED    = 4;
        private const int END       = 5;
        private       int state     = 1;

        public ContentManager contentMain;
        public ContentManager contentTemp;
        public SpriteBatch spriteBatch;

        private int level;

        public StateManager(Game game)
        {
            contentMain = (ContentManager) game.Services.GetService(typeof(ContentManager));
            contentMain.RootDirectory = "Content";
            contentTemp = (ContentManager) game.Services.GetService(typeof(ContentManager));
            contentTemp.RootDirectory = "Level";
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));

            level = 1;

            state = 1;
            //load main content


        } //end StateManager

        void showScene()
        {

        } //end showScene

        void disposeScene()
        {

        } //end disposeScene

        void switchLevel()
        {

        } //end switchLevel

    } //end StateManager
} //end
