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

            /*
             * This is the code to read in levels.
             * The constructor reads in the map data file (.map) located in Content\MapData
             * that is specified by the Content.RootDirectory that is set in the Game3
             * constructor. It also reads in the tile set data and creates a Tile class
             * within the TileMap class that uses the texture that is given to, in this
             * case, tiles and defines the dimensions of each tile in the tile set texture.             * 
             */
        public static TileMap LoadStage(ContentManager Content, string levelNum)
        {
            
            Texture2D tiles = Content.Load<Texture2D>(@"Textures\TileSets\input2");

            return new TileMap("testlevel" + levelNum, Content.RootDirectory, tiles, 48, 48, 0);
        } //end LoadStage

        /* This is the code to set player on the stage.
         * Will take in and xPosition and yPosition and set the player position to
         * those values.
         */
        public static Player LoadPlayer(int xPos, int yPos)
        {

        } //end LoadPlayer

        /* This is the code to set enemies on the stage.
         * Will read in a list of Enemies and take the values stored in the list
         * to place various enemies through out the stage.
         */
        public static void LoadEnemies(List<Enemies> enemies)
        {

        } //end LoadEnemies
    } //end Level
}
