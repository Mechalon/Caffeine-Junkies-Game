#region File Description
/*
 * Player.cs
 * Created by Forrest
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

        Level level;
        public Level Level
        {
            get { return level; }
        }

        int lives;
        public int Lives
        {
            get { return lives; }
        }

        bool alive;
        public bool Alive
        {
            get { return alive; }
        }

        bool grounded;
        public bool Grounded
        {
            get { return grounded; }
        }

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Rectangle BoundingBox
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
        private AnimationSystem sprite;
        private SpriteEffects flip = SpriteEffects.None;

        //Sound
        private SoundEffect deathSound;

        //Movement
        private float movement;
        private bool jumping;
        private bool wasJumping;
        private float jumpTime;
        private float lastBottom;
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
            lives = 3;

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


            deathSound = Level.Content.Load<SoundEffect>("Sounds/death");

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
            ReadInput(keyboardState, gamePadState);
            ApplyPhysics(gameTime);

            Camera.Location = new Vector2(
                MathHelper.Clamp(Camera.Location.X, Position.X - Camera.viewWidth * 3 / 4, Position.X - Camera.viewWidth / 4),
                MathHelper.Clamp(Camera.Location.Y, Position.Y - Camera.viewHeight * 3 / 8, Position.Y - Camera.viewHeight * 5 / 8));

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
        public void ReadInput(KeyboardState keyboardState, GamePadState gamePadState)
        {
            //360 Controller movements
            movement = gamePadState.ThumbSticks.Left.X;

            //Stops animating in place for analog controls
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

            //Keyboard movements (overwrites analog stick)
            if (keyboardState.IsKeyDown(Keys.A) || gamePadState.IsButtonDown(Buttons.DPadLeft))
                movement = -1.0f;
            else if (keyboardState.IsKeyDown(Keys.D) || gamePadState.IsButtonDown(Buttons.DPadRight))
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
                    sprite.PlayAnimation(jump);
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
            Rectangle playerBounds = BoundingBox;
            int leftTile = (int)Math.Floor((float)playerBounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / Tile.WIDTH)) - 1;
            int topTile = (int)Math.Floor((float)playerBounds.Top / Tile.HEIGHT);
            int bottomTile = (int)Math.Ceiling(((float)playerBounds.Bottom / Tile.HEIGHT)) - 1;

            grounded = false;

            for (int y = topTile; y <= bottomTile; ++y)
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    TileCollision collision = Level.GetCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        Rectangle tileBounds = Level.GetBounds(x, y);
                        Vector2 intersectDepth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);

                        //if intersecting
                        if (intersectDepth != Vector2.Zero)
                        {
                            float depthX = Math.Abs(intersectDepth.X);
                            float depthY = Math.Abs(intersectDepth.Y);

                            if (depthX > depthY || collision == TileCollision.Platform)
                            {
                                if (lastBottom <= tileBounds.Top)
                                    grounded = true;

                                if (collision == TileCollision.Impassable || grounded)
                                {
                                    Position = new Vector2(Position.X, Position.Y + intersectDepth.Y);
                                    playerBounds = BoundingBox;
                                } //endif
                            } //end if
                            else if (collision == TileCollision.Impassable)
                            {
                                Position = new Vector2(Position.X + intersectDepth.X, Position.Y);
                                playerBounds = BoundingBox;
                            } //end if
                        } //end if
                    } //end if
                } //end for
            lastBottom = playerBounds.Bottom;

        } //end HandleCollsions
        #endregion

        #region Player Events

        /// <summary>
        /// Called when the player is killed. By enemy or falling.
        /// </summary>
        /// <param name="killedBy">The enemy that killed the player.</param>
        public void OnDeath()
        {
            alive = false;
            lives--;

            deathSound.Play();

            sprite.PlayAnimation(death);
        } //end OnDeath

        /// <summary>
        /// Called when player reaches the end of a level.
        /// </summary>
        public void OnWin()
        {
            sprite.PlayAnimation(win);
        } //end OnWin

        #endregion

        #region Draw

        /// <summary>
        /// Draws the player animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X < 0.0f)
                flip = SpriteEffects.None;
            else if (Velocity.X > 0.0f)
                flip = SpriteEffects.FlipHorizontally;

            sprite.Draw(gameTime, spriteBatch, Position - Camera.Location, flip);
        } //end Draw

        #endregion
    } //end Player
} //end
