#region File Description
/*
 * Animation.cs
 * Created by Forrest
 */
#endregion
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    class Animation
    {
        #region Variables

        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        public bool Loops
        {
            get { return loops; }
        }
        bool loops;

        public int FrameCount
        {
            get{ return Texture.Width / FrameWidth;}
        }
        int frameCount;

        public int FrameWidth
        {
            get { return Texture.Height; }
        }
        int frameWidth;

        public int FrameHeight
        {
            get { return Texture.Height; }
        }
        int frameHeight;

        #endregion

        /// <summary>
        /// Constructs an animation.
        /// </summary>
        /// <param name="texture">
        /// Texture used to animate an object.
        /// </param>
        /// <param name="frameTime">
        /// Time each keyframe in the animation lasts.
        /// </param>
        /// <param name="loop">
        /// Determines whether the animation is continuous or happens once.
        /// </param>
        public Animation(Texture2D texture, float frameTime, bool loops)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.loops = loops;
        } //end Animation

    } //end Animation
} //end
