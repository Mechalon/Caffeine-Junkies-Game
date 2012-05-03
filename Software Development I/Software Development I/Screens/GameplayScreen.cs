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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Software_Development_I
{
    class GameplayScreen : GameScreen
    {
        //Resources for drawing
        ContentManager Content;

        float pauseAlpha;

        private Texture2D lifeIcon;
        private Texture2D heart;
        private Texture2D thumbstick;

        //Global content
        private SpriteFont hudFont;

        //Meta-level game state
        private const int numberOfLevels = 2;
#if WINDOWS || XBOX
        private int levelIndex = OptionsMenuScreen.GameOptions.levelSelected - 2;
#endif
#if WINDOWS_PHONE
        private int levelIndex = PhoneMainMenuScreen.GameOptions.levelSelected - 2;
#endif
        private Level level;

        //store input states once per frame
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private AccelerometerState accelerometerState;

        private bool firstRun = true;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        } //end NinjaGame

        public override void LoadContent()
        {
            if (Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Load fonts
            hudFont = Content.Load<SpriteFont>("Fonts/hud");

            // Load Icons
            lifeIcon = Content.Load<Texture2D>("Sprites/Player/lifeIco");
            heart = Content.Load<Texture2D>("Sprites/Player/heart");
            thumbstick = Content.Load<Texture2D>("Sprites/WindowsPhone/thumbstick");

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
            int oldLives = 3; //variable used to set lives, by default is 3
            levelIndex = (levelIndex + 1) % numberOfLevels;

            if (level != null)
                level.Dispose();

            if (levelIndex > 0) //if this isn't the starting level, set lives to equal the old ones
            {
                if (!firstRun)
                oldLives = level.Player.Lives;
            }

            level = new Level(ScreenManager.Game.Services, levelIndex);

            level.Player.addLives(oldLives); //set the lives
            firstRun = false;
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
            VirtualThumbsticks.Update();

            //Update level, passing GameTime and input states.
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
                //Update level, passing GameTime and input states.
                level.Update(gameTime, keyboardState, gamePadState, touchState,
                         accelerometerState, ScreenManager.Game.Window.CurrentOrientation);
        } //end Update

        /// <summary>
        /// Polls input states and handles player input.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            int playerIndex = (int)ControllingPlayer.Value;

            keyboardState = input.CurrentKeyboardStates[playerIndex];
            gamePadState = input.CurrentGamePadStates[playerIndex];
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

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

            if (input.GamePadWasConnected[playerIndex])
                controlCheck.wasConnected = true;

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }

            if (!gamePadState.IsConnected)
            {
                controlCheck.wasConnected = false;
                input.GamePadWasConnected[playerIndex] = false;
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

            if (VirtualThumbsticks.LeftThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.LeftThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.Green);
            }

            if (VirtualThumbsticks.RightThumbstickCenter.HasValue)
            {
                spriteBatch.Draw(
                    thumbstick,
                    VirtualThumbsticks.RightThumbstickCenter.Value - new Vector2(thumbstick.Width / 2f, thumbstick.Height / 2f),
                    Color.Blue);
            }

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
            int hearts = level.Player.Health;
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

            spriteBatch.Draw(lifeIcon, new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(hudFont, livesRemaining, new Vector2(20 + lifeIcon.Width, 20), Color.White);
            for (int x = 0; x < hearts; x++)
                spriteBatch.Draw(heart, new Vector2(20 + x * heart.Width, 50), Color.White);
        } //end DrawHud
    } //end NinjaGame

    public static class controlCheck
    {
        public static bool wasConnected = false;
    }
} //end namespace