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

        float pauseAlpha;
        private Texture2D lifeIcon;

        //Global content
        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;

        //Meta-level game state
        private const int numberOfLevels = 2;
        private int levelIndex = OptionsMenuScreen.GameOptions.levelSelected - 2;
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

            lifeIcon = Content.Load<Texture2D>("Sprites/Player/lifeIco");

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
            base.Update(gameTime, otherScreenHasFocus, false);


            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
                //Update level, passing GameTime and input states.
                level.Update(gameTime, keyboardState, gamePadState);
        } //end Update

        /// <summary>
        /// Polls input states and handles player input.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            int playerIndex = (int)ControllingPlayer.Value;

            keyboardState = input.CurrentKeyboardStates[playerIndex];
            gamePadState = input.CurrentGamePadStates[playerIndex];

            //if (keyboardState.IsKeyDown(Keys.Escape) ||
              //  gamePadState.Buttons.Back == ButtonState.Pressed)
                //this.Exit();

            if (level.Player.Alive && level.EndReached)
                LoadNextLevel();

            if (!level.Player.Alive)
                if (level.Player.Lives > 0)
                    level.NewLife();
                else
                {
                    ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
                    ReloadLevel();
                }

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
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

            DrawHud(spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        } //end Draw

        private void DrawHud(SpriteBatch spriteBatch)
        {
            Rectangle titleSafeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;
            int width = lifeIcon.Width;
            int height = lifeIcon.Height;
            int left = titleSafeArea.Left;
#if WINDOWS
            int top = titleSafeArea.Bottom - 30;
            Vector2 lifeLocation = new Vector2(titleSafeArea.Left + width + 5, titleSafeArea.Bottom - 30);
#endif
#if XBOX
            int top = titleSafeArea.Bottom;
            Vector2 lifeLocation = new Vector2(titleSafeArea.Left + width + 5, titleSafeArea.Bottom);
#endif
            Rectangle iconLocation = new Rectangle(left, top, width, height);
            string livesRemaining = "x " + level.Player.Lives.ToString();

            spriteBatch.Draw(lifeIcon, iconLocation, Color.White);

            spriteBatch.DrawString(hudFont, livesRemaining, lifeLocation, Color.White);
        } //end DrawHud

    } //end NinjaGame
} //end namespace
