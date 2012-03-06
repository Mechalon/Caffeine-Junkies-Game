using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ninjaStuff
{
    public struct PlayerData{
        public Vector2 Position;
        public int ammo; //ninja star ammo?
        public int health; //the number of hits the player has remaining
        public int lives; //the number of lives the player has
        public bool grounded; //is the player on the ground?
        public float velocity; //velocity of the player in the y direction
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //graphics side stuff
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        int screenWidth;
        int screenHeight;
        SpriteFont font;
        //visual stuff
        Texture2D ninjaTexture;
        Texture2D floorTexture;
        Texture2D backgroundTexture;
        PlayerData player;
        float floor;
        //float jumpHeight;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "Ninja Game";
            floor = 600.0f;
            //jumpHeight = 5.0f;
            base.Initialize();
        }

        private void SetUpPlayer()
        {
            player.ammo = 1;
            player.health = 5;
            player.lives = 5;
            player.Position = new Vector2(100, floorTexture.Height-120);
            player.grounded = false;
            player.velocity = 5.0f;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            backgroundTexture = Content.Load<Texture2D>("background");
            floorTexture = Content.Load<Texture2D>("floor");
            ninjaTexture = Content.Load<Texture2D>("ninja");
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            SetUpPlayer();
            font = Content.Load<SpriteFont>("font1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            ProcessKeyboard();
            gravity(); //handles bringing the character back down and making sure on the ground at the right place
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void gravity()
        {
            if (player.Position.Y < floor)
            {
                player.velocity--;
                player.Position.Y -= player.velocity;
                if (player.velocity == 0)
                    player.grounded = false;
            }
            else
            {
                player.Position.Y = floor;
                player.grounded = true;
            }
        }

        private void ProcessKeyboard()
        {
            KeyboardState key = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            //exit the game when the excape key is pressed
            if (key.IsKeyDown(Keys.Escape) || pad.Buttons.Back == ButtonState.Pressed)
                Exit();
            if (key.IsKeyDown(Keys.D) || pad.DPad.Right == ButtonState.Pressed)
                moveFoward(true);
            if (key.IsKeyDown(Keys.A) || pad.DPad.Left == ButtonState.Pressed)
                moveFoward(false);
            if (key.IsKeyDown(Keys.W) && player.grounded || pad.Buttons.A == ButtonState.Pressed)
                jump();
            else if (player.Position.Y < floor)//checks to see if in the air
                player.grounded = false;
            if (key.IsKeyDown(Keys.S) && player.grounded || pad.DPad.Down == ButtonState.Pressed)
                crouch();
            if (key.IsKeyDown(Keys.Enter) || pad.Buttons.X == ButtonState.Pressed)
                sword();
            if (key.IsKeyDown(Keys.Space) || pad.Buttons.Y == ButtonState.Pressed)
                star();
            if (key.IsKeyDown(Keys.Pause) || pad.Buttons.Start == ButtonState.Pressed)
                pause();
            //controls for the player
        }

        private void pause()
        {
            //pause the game, also displays the controls
        }

        private void star()
        {
            //throw a ninja star
        }

        private void sword()
        {
            //attack with your sword
        }

        private void jump()
        {
            player.velocity = 10.0f;            
        }

        private void crouch()
        {
            //not doing anything at the moment
        }
        private void moveFoward(bool right)
        {
            if (right) player.Position.X += 5;
            else player.Position.X -= 5;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            DrawScenery();
            DrawPlayer();
            DrawText();
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(floorTexture, screenRectangle, Color.White);
        }

        private void DrawPlayer()
        {
            if (player.health > 0)
            {
                int xPos = (int)player.Position.X;
                int yPos = (int)player.Position.Y;
                spriteBatch.Draw(ninjaTexture, new Vector2(xPos + 20, yPos - 10), null,Color.White);
             }
        }

        private void DrawText()
        {

        }
    }
}
