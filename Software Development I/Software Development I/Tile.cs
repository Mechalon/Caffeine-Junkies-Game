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
        public int spacing;

        public Tile(Texture2D tileSetTexture, int width, int height, int spacing)
        {
            this.tileSetTexture = tileSetTexture;
            this.width = width;
            this.height = height;
            this.spacing = spacing;
        } //end Tile

        public Rectangle GetSourceRectangle(int tileIndex)
        {
            int tileX = tileIndex % (tileSetTexture.Width / width);
            int tileY = tileIndex / (tileSetTexture.Width / width);
            return new Rectangle(
                tileX * width + tileX * spacing,
                tileY * height + tileY * spacing,
                width,
                height);
        } //end GetSourceRectangle
            
    } //end class Tile
} //end namespace
