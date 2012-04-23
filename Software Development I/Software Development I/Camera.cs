using Microsoft.Xna.Framework;

/*
 * Revise to follow player
 */
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
                    MathHelper.Clamp(value.X, 0f, worldWidth - viewWidth-32),
                    MathHelper.Clamp(value.Y, 0f, worldHeight - viewHeight-32));
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
            Camera.worldWidth = levelMap.mapWidth * levelMap.tileProperties.width;
            Camera.worldHeight = levelMap.mapHeight * levelMap.tileProperties.height;
            Camera.location = Vector2.Zero;
        } //end InitializeLevel

        /// <summary>
        /// Returns the position in the game world relative to the screen.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - location + displayOffset;
        } //end WorldToScreen

        /// <summary>
        /// Returns the position of the screen relative to the game world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + location - displayOffset;
        } //end ScreenToWorld

        /// <summary>
        /// Allows for camera movement.
        /// </summary>
        /// <param name="offset">
        /// Vector(length/direction) in which the camera is moved.
        /// </param>
        public static void Move(Vector2 offset)
        {
            Location += offset;
        } //end Move
    } //end class Camera
} //end namespace
