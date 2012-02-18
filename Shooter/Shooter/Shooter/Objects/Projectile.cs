// Projectile.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Projectile
    {
        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Repersents the viewable boundary of the game
        Viewport viewport;

        // Get the width of the projectile
        public int Width
        {
            get { return Texture.Width; }
        }

        // Get the height of the projectile
        public int Height
        {
            get { return Texture.Height; }
        }

        // Dertermines how fast the projectile moves
        float projectileMoveSpeed;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            Damage = 2;

            projectileMoveSpeed = 20f;
        }

        public void Update()
        {
            // Projectiles always move to the right
            Position.X += projectileMoveSpeed;

            // Deactive the bullet if it goes out of screen
            if (Position.X + Texture.Width / 2 > viewport.Width)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
