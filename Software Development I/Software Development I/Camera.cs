using Microsoft.Xna.Framework;

namespace Software_Development_I
{
    static class Camera
    {
        public static int viewWidth
        {
            get;
            set;
        } //end viewWidth
        public static int viewHeight
        {
            get;
            set;
        } //end viewHeight
        public static int worldWidth
        {
            get;
            set;
        } //end worldWidth
        public static int worldHeight
        {
            get;
            set;
        } //end worldHeight

        public static Vector2 displayOffset
        {
            get;
            set;
        } //end displayOffset
        public static Vector2 location = Vector2.Zero;

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
            location += offset;
        } //end Move
    } //end class Camera
} //end namespace
