﻿#region File Description
/*
 * NinjaGame.cs
 * 
 * Main game file.
 */
#endregion

using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Software_Development_I
{
    class NinjaGame : Microsoft.Xna.Framework.Game
    {
        //Resources for drawing
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Global content
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;

        //Game state for level switches
        private StateManager gameState;

        //Meta-level game state
        private const int numberOfLevels = 1;
        private int levelIndex = -1;
        private Level level;
        private bool continuePressed;

        //store input states once per frame
        private GamePadState gamePadState;
        private KeyboardState keyboardState;

        int baseOffsetX = -32;
        int baseOffsetY = -64;
        TileMap stage;

        public NinjaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gameState = new StateManager(this);
        } //end Game3

        protected override void Initialize()
        {
            /*
             * This is the code to initialize the camera.
             * ViewWidth and ViewHeight set the viewport in the camera class.
             * WorldWidth and WorldHeight set the dimensions of the level map.
             * DisplayOffset helps the camera stay in bounds.
             */
            Camera.viewWidth = this.graphics.PreferredBackBufferWidth;
            Camera.viewHeight = this.graphics.PreferredBackBufferHeight;
            Camera.worldWidth = stage.mapWidth * stage.tileProperties.width;
            Camera.worldHeight = stage.mapHeight * stage.tileProperties.height;
            Camera.displayOffset = new Vector2(baseOffsetX, baseOffsetY);
            Camera.location = Vector2.Zero;

            base.Initialize();
        } //end Initialize

        protected override void LoadContent()
        {
            //Create Sprite Batch for drawing textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/hud");

            //Load overlays
            winOverlay = Content.Load<Texture2D>("Overlays/win");
            loseOverlay = Content.Load<Texture2D>("Overlays/lose");

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Content.Load<Song>("Sounds/music"));
            }
            catch { }

            LoadNextLevel();

            base.LoadContent();
        } //end LoadContent

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

        protected override void UnloadContent()
        {
            base.UnloadContent();
        } //end UnloadContent

        protected override void Update(GameTime gameTime)
        {
            //Get input states and handle input
            HandleInput();

            gameState.Update(gameTime, keyboardState, gamePadState);

            base.Update(gameTime);
        } //end Update

        private void HandleInput()
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) ||
                gamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (keyboardState.IsKeyDown(Keys.Left))
                Camera.Move(new Vector2(-2, 0));
            if (keyboardState.IsKeyDown(Keys.Right))
                Camera.Move(new Vector2(2, 0));
            if (keyboardState.IsKeyDown(Keys.Up))
                Camera.Move(new Vector2(0, -2));
            if (keyboardState.IsKeyDown(Keys.Down))
                Camera.Move(new Vector2(0, 2));
        } //end HandleInput

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            //Level.Draw(gameTime, spriteBatch);

            DrawHud();

            spriteBatch.End();

            base.Draw(gameTime);
        } //end Draw

        private void DrawHud()
        {

        } //end DrawHud
    } //end class Game3
} //end namespace
