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
    class GameplayScreen : GameScreen
    {
        //Resources for drawing
        ContentManager Content;
        SpriteFont gameFont;

        //Global content
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;

        //Meta-level game state
        private const int numberOfLevels = 2;
        private int levelIndex = -1;
        private Level level;
        private bool continuePressed;

        //store input states once per frame
        private GamePadState gamePadState;
        private KeyboardState keyboardState;


        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            //Content.RootDirectory = "Content";
        } //end NinjaGame

        //protected override void Initialize()
        //{
            //Camera.InitializeScreen(graphics);

            //base.Initialize();
        //} //end Initialize

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");
            //Create Sprite Batch for drawing textures
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/hud");

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

            //Camera.InitializeScreen(graphics);

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

            level = new Level(ScreenManager.Game.Services, levelIndex);

        } //end LoadNextLevel()

        /// <summary>
        /// Reloads the current level.
        /// </summary>
        public void ReloadLevel()
        {
            levelIndex--;
            LoadNextLevel();
        } //end ReloadLevel

        public override void UnloadContent()
        {
            Content.Unload();
        } //end UnloadContent

        /// <summary>
        /// Gather input, update game states, check collision, play audio.
        /// </summary>
        /// <param name="gameTime">
        /// Snapshot of timing values.
        /// </param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            //Get input states and handle input
            //HandleInput();

            //Update level, passing GameTime and input states.
            level.Update(gameTime, keyboardState, gamePadState);

            base.Update(gameTime, otherScreenHasFocus, false);
        } //end Update

        /// <summary>
        /// Polls input states and handles player input.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            //if (keyboardState.IsKeyDown(Keys.Escape) ||
              //  gamePadState.Buttons.Back == ButtonState.Pressed)
                //this.Exit();

            if (level.Player.Alive && level.EndReached)
                LoadNextLevel();

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
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

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
