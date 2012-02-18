using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Enemy
    {
        // Animation repersenting the enemy
        public Animation EnemyAnimation;
        
        // The position of the enemy ship relative to the top left corner of the screen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // The hit points of the enemy, if this goes to zero, the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;
        
        // Get the width of the enemy ship
        // This is actually REALLY convient, considering we can actually use this with ANY enemy type
        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }

        // Get the height of the enemy ship
        // Considering we put this in when an enemy is made, this is prototyped.  How cool is that?
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }

        // The speed at which the enemy moves
        float enemyMoveSpeed;

        public void Initalize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            EnemyAnimation = animation;

            // Set the position of the enemy
            Position = position;

            // We initalize the enemy to be active so it will be updated in the game
            Active = true;

            // Set the health of the enemy
            Health = 10;

            // Set the amount of damage the enemy can do
            Damage = 10;

            // Set how fast the enemy moves
            enemyMoveSpeed = 6f;

            // Set the score value of the enemy
            Value = 100;
        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= enemyMoveSpeed;

            // Update the position of the Animation
            EnemyAnimation.Position = Position;

            // Update Animation
            EnemyAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactive it
            // Holy crap, this is it for garbage collection?!
            if (Position.X < -Width || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this object from the
                // active game list
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            EnemyAnimation.Draw(spriteBatch);
        }
    }
}
