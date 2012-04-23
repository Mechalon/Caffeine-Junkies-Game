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

        /// <summary>
        /// Creates individual tiles from a texture for level drawing.
        /// </summary>
        /// <param name="tileSetTexture">
        /// Texture used to create tile textures.
        /// </param>
        /// <param name="width">
        /// Width of the tile.
        /// </param>
        /// <param name="height">
        /// Height of the tile.
        /// </param>
        /// <param name="spacing">
        /// Spacing between each tile.
        /// </param>
        public Tile(Texture2D tileSetTexture, int width, int height, int spacing)
        {
            this.tileSetTexture = tileSetTexture;
            this.width = width;
            this.height = height;
            this.spacing = spacing;
        } //end Tile

        /// <summary>
        /// Rectangle area within the texture defining the tiles texture.
        /// </summary>
        /// <param name="tileIndex">
        /// The tile number in the texture. Numbering starts at top left and goes horizontal then vertical.
        /// </param>
        /// <returns>
        /// Returns the rectangle from texture.
        /// </returns>
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
