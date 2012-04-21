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

        

        public StateManager(Game game)
        {
            state = 1;
        } //end StateManager

    } //end StateManager
} //end
