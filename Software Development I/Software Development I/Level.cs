using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Software_Development_I
{
    class Level : IDisposable
    {
        public ContentManager content;

        private TileMap levelMap;
        private SoundEffect exampleSound;

        #region Loading

        /// <summary>
        /// Creates a new level
        /// </summary>
        /// <param name="service">
        /// Service provider used for a ContentManager
        /// </param>
        /// <param name="filePath">
        /// String of path to file containing tile placement data
        /// </param>

        public Level(IServiceProvider service, String filePath, int levelIndex)
        {
            //Create content manager for current level.
            content = new ContentManager(service, "Content");

            //Load level map
            levelMap = new TileMap(filePath, content.Load<Texture2D>("Tiles/tiles"), 32, 32, 2);

            //Load sounds
            //exampleSound = content.Load<SoundEffect>("Sounds/example");

            //Initialize Camera
            Camera.InitializeLevel(levelMap);
        } //end Level

        public void Dispose()
        {
            content.Unload();
        } //end DisposeStage()

        #endregion

        #region Draw

        /// <summary>
        /// Draw the level
        /// </summary>

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            levelMap.Draw(gameTime, spriteBatch);
        } //end Draw

        #endregion

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
