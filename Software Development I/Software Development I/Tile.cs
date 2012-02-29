using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    class Tile
    {
        public Texture2D tileSetTexture;
        public int width;
        public int height;

        public Tile(Texture2D tileSetTexture, int width, int height)
        {
            this.tileSetTexture = tileSetTexture;
            this.width = width;
            this.height = height;
        } //end Tile

        public Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileX = tileIndex % (tileSetTexture.Width / width);
            int tileY = tileIndex / (tileSetTexture.Width / width);
            return new Rectangle(
                tileX * width,
                tileY * height,
                width,
                height);
        } //end GetSourceRectangle
            
    } //end class Tile
} //end namespace
