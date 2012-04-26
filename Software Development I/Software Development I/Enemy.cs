//modified enemy class for animation 

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RotateSpriteWindows
{
    class Enemy
    {
        //Animation representing the enemy
        public Animation EnemyAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        // Get the width of the enemy ship
        public int Width;
        //{
        //    get { return EnemyTexture.FrameWidth; }
        //}

        // Get the height of the enemy ship
        public int Height;
        //{
        //    get { return EnemyTexture.FrameHeight; }
        //}

        // The speed at which the enemy moves
        public float enemyMoveSpeed;

        public int framecount;

        public int frametime;

        public float targetangle; //rotation angle for targeting


        //public void Initialize(Animation animation, Vector2 position)
        public void Initialize(Vector2 position)
        {
            // Load the enemy ship texture
            //EnemyAnimation = animation;

            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initalize the animation with the correct animation information
            //enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;

            Health = 10; // just for simplicity's sake 
            enemyMoveSpeed = 0;

            Width = 47; // framewidth also

            Height = 61;//frame height

            framecount = 1;

            frametime = 1000;

            targetangle = 0;

        }


        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= enemyMoveSpeed;

            //float x, y, theta;
            //float xspeed, yspeed;
            //D3DXVECTOR3 v; //location vector
 
            //v.x = cannon->m_structLocation.x + 20;
            //v.y = cannon->m_structLocation.y;
            //v.z = cannon->m_structLocation.z;

            //x = fabs(cannon->m_structLocation.x - m_pPlayerObject->m_structLocation.x);
            //y = fabs(cannon->m_structLocation.y - m_pPlayerObject->m_structLocation.y);

            //theta = atan(y/x);

            //if(cannon->m_structLocation.x > m_pPlayerObject->m_structLocation.x)
            //{
            //    xspeed = -25 * cos(theta);
            //}
            //else
            //{
            //    xspeed = 25 * cos(theta);
            //}

            //if(cannon->m_structLocation.y > m_pPlayerObject->m_structLocation.y)
            //{
            //    yspeed = 25 * sin(theta);
            //}
            //else
            //{
            //    yspeed = -25 * sin(theta);
            //}

            //create(ENEMY_STAR, "enemy_star", v, xspeed, yspeed);
            //}

            // Update the position of the Animation
            //EnemyAnimation.Position = Position;

            // Update Animation
            //EnemyAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (Position.X < -Width || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            //EnemyAnimation.Draw(spriteBatch);
        }

    }
}


/*
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RotateSpriteWindows
{
    class Enemy
    {
        //Animation representing the enemy
        public Animation EnemyAnimation;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // The amount of score the enemy will give to the player
        public int Value;

        // Get the width of the enemy ship
        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }

        // The speed at which the enemy moves
        float enemyMoveSpeed;


        public void Initialize(Animation animation, Vector2 position)
        {
            // Load the enemy ship texture
            EnemyAnimation = animation;

            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
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

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (Position.X < -Width || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
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
}*/
