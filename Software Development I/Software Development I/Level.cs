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
        #region Variables

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public Player Player
        {
            get { return player; }
        }
        Player player;

        public bool EndReached
        {
            get { return endReached; }
        }
        bool endReached;

        private TileMap levelMap;
        private Vector2 start;
        private Point end = new Point(-1, -1);
        private SoundEffect exampleSound;

        #endregion

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
            levelMap = new TileMap(filePath, content.Load<Texture2D>("Tiles/tiles"));

            
            //Load sounds
            //exampleSound = content.Load<SoundEffect>("Sounds/example");

            //Initialize Camera
            Camera.InitializeLevel(levelMap);
            player = new Player(this, Camera.Location);
        } //end Level

        /// <summary>
        /// Garbage collection for current level content
        /// </summary>
        public void Dispose()
        {
            content.Unload();
        } //end DisposeStage()

        #endregion

        #region Collision

        public TileCollision GetCollision(int x, int y)
        {
            return levelMap.rows[y].columns[x].collision;
        } //end GetCollision

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Tile.WIDTH, y * Tile.HEIGHT, Tile.WIDTH, Tile.HEIGHT);
        } //end GetBounds

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the game world
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (Player.Alive)
                if (endReached)
                {

                } //end if
                else
                {
                    Player.Update(gameTime, keyboardState, gamePadState);

                    

                    //if Player.BoundingRectangle.Top is lower than cam bot
                        //player.OnKilled(null);

                    UpdateEnemies(gameTime);

                    if (Player.Alive && Player.Grounded && Player.BoundingRectangle.Contains(end))
                        OnEndReached();

                    
                } //end else

        } //end Update

        /// <summary>
        /// Animates enemies and checks for player collision
        /// </summary>
        private void UpdateEnemies(GameTime gameTime)
        {
            //foreach (Enemy enemy in enemies)
                //enemy.Update(gameTime)

                //if (enemy.BoundingRectangle.Intersects(player.BoundingRectangle))
                    //player.OnKilled(enemy);
        } //end UpdateEnemeies

        /// <summary>
        /// Called when player reaches the end of a level
        /// </summary>
        private void OnEndReached()
        {
            //player.OnExitReached();
            //exitReachedSound.Play();
            endReached = true;
        } //end OnExitReached

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

    } //end Level
}
