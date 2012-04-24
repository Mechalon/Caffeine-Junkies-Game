using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    /// <summary>
    /// Controls animation for a player.
    /// </summary>
    struct AnimationChanging
    {
        #region Variables

        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2, Animation.FrameHeight); }
        }

        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        private float time;

        #endregion

        /// <summary>
        /// Starts playing an animation.
        /// </summary>
        /// <param name="animation">
        /// Animation to try and start playing.
        /// </param>
        public void PlayAnimation(Animation animation)
        {
            if (Animation != animation)
            {
                this.animation = animation;
                this.frameIndex = 0;
                this.time = 0.0f;
            } //end if
        } //end PlayAnimation

        /// <summary>
        /// Updates and draws the animation keyframe.
        /// </summary>
        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation != null)
            {
                time += (float)gametime.ElapsedGameTime.TotalSeconds;

                while (time > Animation.FrameTime)
                {
                    time -= Animation.FrameTime;

                    if (Animation.Loops)
                        frameIndex = (frameIndex + 1) % Animation.FrameCount;
                    else
                        frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                } //end while

                Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

                spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
            } //end if
        } //end Draw
    } //end AnimationPlayer
}
