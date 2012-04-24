using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    /// <summary>
    /// Controls collision detection and behaviors of a tile.
    /// </summary>
    enum TileCollision
    {
        /// <summary>
        /// Tile in which the player can pass through freely.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// Tile in which the players' movement is slowed.
        /// </summary>
        Slow = 1,

        /// <summary>
        /// Solid tile in which the player can not pass through at all.
        /// </summary>
        Impassable = 2,

        /// <summary>
        /// Platform tile in which the player can both stand on and pass through.
        /// </summary>
        Platform = 3,
    } //end TileCollision

    class Tile
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 32;
        public const int SPACING = 2;
        public static readonly Vector2 size = new Vector2(WIDTH, HEIGHT);

        public int tileID;
        public TileCollision collision;

        /// <summary>
        /// Creates a tile. Multiple tiles can be used to construct a level.
        /// </summary>
        /// <param name="tileID">
        /// Placement of the tile within the tile sheet.
        /// </param>
        /// <param name="collision">
        /// Represents the type of player interactions happen with each tile.
        /// </param>
        public Tile(int tileID, TileCollision collision)
        {
            this.tileID = tileID;
            this.collision = collision;
        } //end Tile

        public Rectangle GetSourceRectangle(Texture2D texture)
        {
            int tileX = (tileID-1) % (texture.Width / WIDTH);
            int tileY = (tileID-1) / (texture.Width / WIDTH);
            return new Rectangle(
                tileX * WIDTH + tileX * SPACING,
                tileY * HEIGHT + tileY * SPACING,
                WIDTH,
                HEIGHT);
        }
    } //end Tile
} //end namespace
