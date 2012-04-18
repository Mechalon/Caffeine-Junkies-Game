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
        public ContentManager contentPerm;
        public ContentManager contentTemp;
        public SpriteBatch spriteBatch;

        public StateManager(Game game)
        {
            contentPerm = (ContentManager) game.Services.GetService(typeof(ContentManager));
            contentPerm.RootDirectory = "Content";
            contentTemp = (ContentManager) game.Services.GetService(typeof(ContentManager));
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));

            //load content


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
