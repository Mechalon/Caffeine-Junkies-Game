#region File Description
/*
 * NinjaGame.cs
 * Created by XNA
 * Modified by Forrest
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

        //Meta-level game state
        private const int numberOfLevels = 1;
        private int levelIndex = -1;
        private Level level;
        private bool continuePressed;

        //store input states once per frame
        private GamePadState gamePadState;
        private KeyboardState keyboardState;


        public NinjaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        } //end NinjaGame

        protected override void Initialize()
        {
            Camera.InitializeScreen(graphics);

            base.Initialize();
        } //end Initialize

        protected override void LoadContent()
        {
            //Create Sprite Batch for drawing textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load fonts
            //hudFont = Content.Load<SpriteFont>("Fonts/hud");

            //Load overlays
            //winOverlay = Content.Load<Texture2D>("Overlays/win");
            //loseOverlay = Content.Load<Texture2D>("Overlays/lose");

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Content.Load<Song>("Sounds/music"));
            }
            catch { }

            LoadNextLevel();

            base.LoadContent();
        } //end LoadContent

        /// <summary>
        /// Loads the next level.
        /// </summary>
        public void LoadNextLevel()
        {
            //Set the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            if (level != null)
                level.Dispose();

            //Load the level
            string levelPath = string.Format("Content/Levels/{0}.map", levelIndex);
            
            level = new Level(Services, levelPath, levelIndex);

        } //end LoadNextLevel()

        /// <summary>
        /// Reloads the current level.
        /// </summary>
        public void ReloadLevel()
        {
            levelIndex--;
            LoadNextLevel();
        } //end ReloadLevel

        protected override void UnloadContent()
        {
            base.UnloadContent();
        } //end UnloadContent

        /// <summary>
        /// Gather input, update game states, check collision, play audio.
        /// </summary>
        /// <param name="gameTime">
        /// Snapshot of timing values.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            //Get input states and handle input
            HandleInput();

            //Update level, passing GameTime and input states.
            level.Update(gameTime, keyboardState, gamePadState);

            base.Update(gameTime);
        } //end Update

        /// <summary>
        /// Polls input states and handles player input.
        /// </summary>
        private void HandleInput()
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) ||
                gamePadState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (!level.Player.Alive)
                if (level.Player.Lives > 0)
                    level.NewLife();
                else
                    ReloadLevel();
        } //end HandleInput

        /// <summary>
        /// Draws all the games components.
        /// </summary>
        /// <param name="gameTime">
        /// Snapshot of timing values.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            level.Draw(gameTime, spriteBatch);

            DrawHud();

            spriteBatch.End();

            base.Draw(gameTime);
        } //end Draw

        private void DrawHud()
        {

        } //end DrawHud

    } //end NinjaGame
} //end namespace
