#region File Description
/*
 * Level.cs
 * Created by Forrest
 */
#endregion

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Software_Development_I
{
    class Level : IDisposable
    {
        #region Variables

        ContentManager content;
        public ContentManager Content
        {
            get { return content; }
        }

        Player player;
        public Player Player
        {
            get { return player; }
        }

        bool endReached;
        public bool EndReached
        {
            get { return endReached; }
        }

        private TileMap levelMap;
        private Vector2 start = new Vector2(-1, -1);
        private Vector2 checkVec = new Vector2(-1, -1);
        private Point checkPoint = new Point(-1, -1);
        private Point end = new Point(-1, -1);
        private Point freeLife = new Point(-1, -1);

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
        public Level(IServiceProvider service, int levelIndex)
        {
            //Create content manager for current level.
            content = new ContentManager(service, "Content");

            //Load level map
            levelMap = new TileMap(this, levelIndex, content.Load<Texture2D>("Tiles/tiles"));

            //Initialize Camera
            Camera.InitializeLevel(levelMap);
            player = new Player(this, start);
        } //end Level

        public void SetStart(int x, int y)
        {
            start = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
        } // end SetPlayer

        public void SetCheck(int x, int y)
        {
            checkVec = RectangleExtensions.GetBottomCenter(GetBounds(x, y));
            checkPoint = GetBounds(x, y).Center;
        } //end SetCheck

        public void SetEnd(int x, int y)
        {
            end = GetBounds(x, y).Center;
        } //end SetExit

        public void oneUp(int x, int y)
        {
            freeLife = GetBounds(x, y).Center;
        }

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
            if (x < 0 || x >= Camera.worldWidth / Tile.WIDTH)
                return TileCollision.Impassable;
            if (y < 0 || y >= Camera.worldHeight / Tile.HEIGHT)
                return TileCollision.Passable;

            return levelMap.rows[y].columns[x].collision;
        } //end GetCollision

        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle((x) * Tile.WIDTH, (y) * Tile.HEIGHT, Tile.WIDTH, Tile.HEIGHT);
        } //end GetBounds

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the game world
        /// </summary>
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            if (Player.Alive)
                if (endReached)
                {

                } //end if
                else
                {
                    Player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

                    if (Player.BoundingBox.Top >= Camera.worldHeight)
                        Player.OnDeath();

                    UpdateEnemies(gameTime);

                    if (Player.Alive && Player.BoundingBox.Contains(checkPoint))
                    {
                        levelMap.rows[checkPoint.Y / Tile.HEIGHT].columns[checkPoint.X / Tile.WIDTH].tileID = 5;
                        start = checkVec;
                    }

                    if (Player.Alive && Player.Grounded && Player.BoundingBox.Contains(end))
                        OnEndReached();

                    if (Player.Alive && Player.BoundingBox.Contains(freeLife))
                    {
                        levelMap.rows[freeLife.Y / Tile.HEIGHT].columns[freeLife.X / Tile.WIDTH].tileID = 0;
                        Player.addLives(1);
                        freeLife = new Point(-1, -1);
                    }
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

        #endregion

        #region Level Events

        /// <summary>
        /// Starts the player back at the beginning with no penalty.
        /// </summary>
        public void NewLife()
        {
            Player.Reset(start);
        }

        /// <summary>
        /// Called when player reaches the end of a level
        /// </summary>
        private void OnEndReached()
        {
            endReached = true;
            //player.OnWin();
            //exitReachedSound.Play();
        } //end OnExitReached

        #endregion

        #region Draw

        /// <summary>
        /// Draw the level
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            levelMap.Draw(spriteBatch);
            Player.Draw(gameTime, spriteBatch);
        } //end Draw

        #endregion

    } //end Level
}
