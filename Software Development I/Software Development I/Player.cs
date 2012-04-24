#region File Description
/*
 * Player.cs
 * 
 * Creates and manages the player or players.
 */
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Software_Development_I
{
    /// <summary>
    /// The ninja!
    /// </summary>
    class Player
    {
        #region Variables

        //Constants for horizontal movement
        private const float MOVEACCEL = 13000.0f;
        private const float MAXSPEED = 1750.0f;
        private const float GROUNDDRAG = 0.48f;
        private const float AIRDRAG = 0.58f;

        //Constants for vertical movement
        private const float JUMPTIME = 0.35f;
        private const float JUMPVELOCITY = -3500.0f;
        private const float GRAVITY = 3400.0f;
        private const float MAXFALLSPEED = 550.0f;
        private const float JUMPCONTROL = 0.14f;
        
        public Level Level
        {
            get { return level; }
        }
        Level level;

        public bool Alive
        {
            get { return alive; }
        }
        bool alive;

        public bool Grounded
        {
            get { return grounded; }
        }
        bool grounded;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        } 

        //Animation
        private Animation idle;
        private Animation run;
        private Animation jump;
        private Animation win;
        private Animation death;
        private AnimationChanging sprite;
        private SpriteEffects flip = SpriteEffects.None;

        //Sound        
        private SoundEffect jumpSound;
        private SoundEffect fallSound;
        private SoundEffect deathSound;

        //Movement
        private float movement;
        private bool jumping;
        private bool wasJumping;
        private float jumpTime;
        private Rectangle localBounds;

        #endregion

        #region Loading

        /// <summary>
        /// Constructor to instantiate a new player.
        /// </summary>
        /// <param name="level">
        /// Get level to use its ContentManager.
        /// </param>
        /// <param name="position">
        /// Set starting position of the player.
        /// </param>
        public Player(Level level, Vector2 position)
        {
            this.level = level;

            //Load Player content:
            idle = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Idle"), 0.1f, true);
            run = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Run"), 0.1f, true);
            jump = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Jump"), 0.1f, false);
            win = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Win"), 0.1f, false);
            death = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/Die"), 0.1f, false);

            int width = (int)(idle.FrameWidth * 0.4);
            int left = (idle.FrameWidth - width) / 2;
            int height = (int)(idle.FrameWidth * 0.8);
            int top = idle.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
            //Sounds

            Reset(position);

        } //end Player

        /// <summary>
        /// Reset the player and make alive.
        /// </summary>
        /// <param name="position">
        /// Position the player resets to.
        /// </param>
        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            alive = true;
            sprite.PlayAnimation(idle);
        } //end Reset

        #endregion

        #region Update and Movement

        /// <summary>
        /// Updates input, physics and animations for the player.
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            GetInput(keyboardState, gamePadState);
            ApplyPhysics(gameTime);

            if (Alive && Grounded)
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                    sprite.PlayAnimation(run);
                else
                    sprite.PlayAnimation(idle);

            movement = 0.0f;
            jumping = false;

        } //end Update

        /// <summary>
        /// Handles input for horizontal movement and jumps.
        /// </summary>
        public void GetInput(KeyboardState keyboardState, GamePadState gamePadState)
        {
            //360 Controller movements
            movement = gamePadState.ThumbSticks.Left.X;

            //Stops animating in place for analog controls
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            //Keyboard movements (overwrites analog stick)
            if (keyboardState.IsKeyDown(Keys.A))
                movement = -1.0f;
            else if (keyboardState.IsKeyDown(Keys.D))
                movement = 1.0f;

            //checks for jumps
            jumping = gamePadState.IsButtonDown(Buttons.A) ||
                      keyboardState.IsKeyDown(Keys.Space);
        } //end GetInput

        /// <summary>
        /// Updates player based on input and physics.
        /// </summary>
        public void ApplyPhysics(GameTime gameTime)
        {
            float totalTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = Position;

            //Set velocity based on movement and gravity.
            velocity.X += movement * MOVEACCEL * totalTime;
            velocity.Y = MathHelper.Clamp(velocity.Y + GRAVITY * totalTime, -MAXFALLSPEED, MAXFALLSPEED);
            velocity.Y = Jump(velocity.Y, gameTime);

            //Apply horizontal damping.
            if (Grounded)
                velocity.X *= GROUNDDRAG;
            else
                velocity.X *= AIRDRAG;

            //Set top horizontal speed.
            velocity.X = MathHelper.Clamp(velocity.X, -MAXSPEED, MAXSPEED);

            //Apply velocity to players position.
            Position += velocity * totalTime;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions();

            //Cancel velocity based on collisions
            if (Position.X == previousPosition.X)
                velocity.X = 0.0f;
            if (Position.Y == previousPosition.Y)
                velocity.Y = 0.0f;
        } //end ApplyPhysics

        /// <summary>
        /// Handles jump input.
        /// </summary>
        /// <param name="velocityY">
        /// Players' current vertical velocity.
        /// </param>
        private float Jump(float velocityY, GameTime gameTime)
        {
            if (jumping)
            {
                //Start/Continue jumping
                if ((!wasJumping && grounded) || jumpTime > 0.0f)
                {
                    //play jump sound if new jump

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //animate jump

                } //end if

                //Going up
                if (0.0f < jumpTime && jumpTime <= JUMPTIME)
                    velocityY = JUMPVELOCITY * (1.0f - (float)Math.Pow(jumpTime / JUMPTIME, JUMPCONTROL));
                //Peak of jump
                else
                    jumpTime = 0.0f;
            } //end if
            //Cancel jump
            else
                jumpTime = 0.0f;

            wasJumping = jumping;

            return velocityY;
        } //end Jump

        /// <summary>
        /// Handles collision between the player ad
        /// </summary>
        private void HandleCollisions()
        {

        } //end HandleCollsions
        #endregion

        #region Player Events

        /// <summary>
        /// Called when the player is killed. By enemy or falling.
        /// </summary>
        /// <param name="killedBy">The enemy that killed the player.</param>
        //        public void OnDeath(Enemy killedBy)
        //        {
        //            alive = false;

        //play fall or death sound

        //animate
        //        } //end OnDeath

        /// <summary>
        /// Called when player reaches the end of a level.
        /// </summary>
        public void OnWin()
        {
            //animate
        } //end OnWin

        #endregion

        #region Draw

        /// <summary>
        /// Draws the player animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X > 0.0f)
                flip = SpriteEffects.None;
            else if (Velocity.X < 0.0f)
                flip = SpriteEffects.FlipHorizontally;

            sprite.Draw(gameTime, spriteBatch, Position, flip);
        } //end Draw

        #endregion
    } //end Player
} //end
