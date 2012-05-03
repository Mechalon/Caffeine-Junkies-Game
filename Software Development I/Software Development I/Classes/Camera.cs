#region File Description
/*
 * Camera.cs
 * Created by Forrest
 */

#endregion

using Microsoft.Xna.Framework;

namespace Software_Development_I
{
    static class Camera
    {
        public static int viewWidth { get; set; }
        public static int viewHeight { get; set; }
        public static int worldWidth { get; set; }
        public static int worldHeight { get; set; }

        public static Vector2 displayOffset { get; set; }

        public static Vector2 location = Vector2.Zero;
        public static Vector2 Location
        {
            get
            {
                return location;
            } //end get
            set
            {
                location = new Vector2(
                    MathHelper.Clamp(value.X, 0f, worldWidth - viewWidth - Tile.WIDTH),
                    MathHelper.Clamp(value.Y, 0f, worldHeight - viewHeight - Tile.HEIGHT));
            } //end get
        }

        /// <summary>
        /// Initializes the screen's width and height.
        /// </summary>
        public static void InitializeScreen(GraphicsDeviceManager graphics)
        {
            Camera.viewWidth = graphics.PreferredBackBufferWidth;
            Camera.viewHeight = graphics.PreferredBackBufferHeight;
        } //end InitializeScreen

        /// <summary>
        /// Initializes the screen's min and max positions for the level.
        /// </summary>
        /// <param name="levelMap">
        /// Level the camera is displaying.
        /// </param>
        public static void InitializeLevel(TileMap levelMap)
        {
            Camera.worldWidth = levelMap.mapWidth * Tile.WIDTH;
            Camera.worldHeight = levelMap.mapHeight * Tile.HEIGHT;
            Camera.location = Vector2.Zero;
        } //end InitializeLevel
    } //end class Camera
} //end namespace
