﻿#region File Description
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
using Microsoft.Xna.Framework.Input.Touch;

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

        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;

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

        int health;
        public int Health
        {
            get { return health; }
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

        //Health and Lives
        private GameTime curTime;
        float lastDamage = 0;

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
            health = 5;
            curTime = new GameTime();

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
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            ReadInput(keyboardState, gamePadState, touchState, accelState, orientation);
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
        public void ReadInput(
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            //360 Controller movements
            movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

#if WINDOWS_PHONE
            if (PhoneMainMenuScreen.GameOptions.controlOptions != 2)
                movement = VirtualThumbsticks.LeftThumbstick.X * MoveStickScale;
#endif

            //Stops animating in place for analog controls
            if (Math.Abs(movement) < 0.5f)
                movement = 0.0f;

#if WINDOWS_PHONE
            if (PhoneMainMenuScreen.GameOptions.controlOptions != 1)
            {
                // Move the player with accelerometer
                if (Math.Abs(accelState.Acceleration.Y) > 0.10f)
                {
                    // set our movement speed
                    movement = MathHelper.Clamp(-accelState.Acceleration.Y * AccelerometerScale, -1f, 1f);

                    // if we're in the LandscapeLeft orientation, we must reverse our movement
                    if (orientation == DisplayOrientation.LandscapeRight)
                        movement = -movement;
                }
            }
#endif

            //Keyboard movements (overwrites analog stick)
            if (keyboardState.IsKeyDown(Keys.A) || gamePadState.IsButtonDown(Buttons.DPadLeft))
                movement = -1.0f;
            else if (keyboardState.IsKeyDown(Keys.D) || gamePadState.IsButtonDown(Buttons.DPadRight))
                movement = 1.0f;

            //checks for jumps
            jumping = gamePadState.IsButtonDown(Buttons.A) ||
                      keyboardState.IsKeyDown(Keys.Space) ||
                      keyboardState.IsKeyDown(Keys.Down);
#if WINDOWS_PHONE
            if (PhoneMainMenuScreen.GameOptions.controlOptions == 2)
            {
                jumping = touchState.AnyTouch();
            }

            if (PhoneMainMenuScreen.GameOptions.controlOptions != 2)
            {
                if (VirtualThumbsticks.RightThumbstickCenter != null)
                {
                    jumping = true;
                }
            }
#endif
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
            float totalTime = (float)curTime.ElapsedGameTime.TotalSeconds;

            grounded = false;

            for (int y = topTile; y <= bottomTile; ++y)
            {
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
                                } //end if

                                if (collision == TileCollision.Hurt)
                                {
                                    Position = new Vector2(Position.X, Position.Y + intersectDepth.Y);

                                    playerBounds = BoundingBox;

                                    takeDamage(1);
                                    lastDamage = totalTime;
                                } //end if
                            } //end if

                            else
                            {
                                if (collision == TileCollision.Impassable)
                                {
                                    Position = new Vector2(Position.X + intersectDepth.X, Position.Y);
                                    playerBounds = BoundingBox;
                                    //if (velocity.Y == MAXFALLSPEED) takeDamage(1); damage if the player falls too fast?
                                } //end if

                                if (collision == TileCollision.Hurt)
                                {
                                    Position = new Vector2(Position.X + intersectDepth.X, Position.Y);
                                    playerBounds = BoundingBox;

                                    takeDamage(1);
                                    lastDamage = totalTime;

                                } //end if

                            } //end else
                        } //end if
                    } //end if
                } //end for
            } // End for

            lastBottom = playerBounds.Bottom;
        } //end HandleCollsions

        #endregion

        #region Player Events

        public void takeDamage(int damage)
        {
            health -= damage; //player loses life equal to damage
            if (health <= 0)
                OnDeath();  //if health is gone, kill the player
        }

        public void knockBack(int push)
        {
            movement = push;
        }

        public void addLives(int x)
        {
            lives += x;
        }

        /// <summary>
        /// Called when the player is killed. By enemy or falling.
        /// </summary>
        /// <param name="killedBy">The enemy that killed the player.</param>
        public void OnDeath()
        {
            alive = false;
            lives--;
            health = 5;
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
