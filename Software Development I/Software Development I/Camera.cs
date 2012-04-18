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

        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return worldPosition - location + displayOffset;
        } //end WorldToScreen

        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return screenPosition + location - displayOffset;
        } //end ScreenToWorld

        public static void Move(Vector2 offset)
        {
            Location += offset;
        } //end Move
    } //end class Camera
} //end namespace
