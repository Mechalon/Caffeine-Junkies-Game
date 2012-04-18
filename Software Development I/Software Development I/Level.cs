using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    static class Level
    {
        public static ContentManager levelContent;
            /*
             *
             */
        public static TileMap LoadStage(ContentManager Content, string levelName, Texture2D tileSheet)
        {
            levelContent.RootDirectory = "Content";
            return new TileMap(levelName, Content.RootDirectory, tileSheet, 32, 32, 2);
        } //end LoadStage

        public static void DisposeStage()
        {

        } //end DisposeStage()

        /* This is the code to set player on the stage.
         * Will take in and xPosition and yPosition and set the player position to
         * those values.
         *
        public static Player LoadPlayer(int xPos, int yPos)
        {

        } //end LoadPlayer

        /* This is the code to set enemies on the stage.
         * Will read in a list of Enemies and take the values stored in the list
         * to place various enemies through out the stage.
         *
        public static void LoadEnemies(List<Enemies> enemies)
        {

        } //end LoadEnemies
         * 
         */

    } //end Level
}
