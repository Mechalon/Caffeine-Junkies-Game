using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Software_Development_I
{
    class PhysicsGameObject
    {
        Texture2D texture;
        public Body objectBody;
        Fixture objectFixture;
        const float PIXELS_PER_METER = 20;
        const float RESTITUTION = 0.5f;
        const float FRICTION = 0.3f;
        float scale;
        float density;
        float depth;

        public PhysicsGameObject(Texture2D texture, World world, bool isStatic,
            Vector2 position)
        {
            this.texture = texture;
            scale = 1;
            depth = 1;
            density = 1;

            objectBody = new Body(world);
            objectFixture = FixtureFactory.AttachRectangle(
                texture.Width / PIXELS_PER_METER,
                texture.Height / PIXELS_PER_METER,
                1,
                Vector2.Zero,
                objectBody);
            objectBody.Position = position;
            if (isStatic)
                objectBody.BodyType = BodyType.Static;
            else
                objectBody.IsStatic = false;

            objectFixture.Restitution = RESTITUTION;
            objectFixture.Friction = FRICTION;


        } //end PhysicsGameObject

        public PhysicsGameObject(Texture2D texture, World world, bool isStatic,
            Vector2 position, float scale, float density, float depth)
        {
            this.texture = texture;
            this.scale = scale;
            this.density = density;
            this.depth = depth;

            //loads body details
            objectBody = new Body(world);
            objectBody.Position = position;
            if (isStatic)
                objectBody.BodyType = BodyType.Static;
            else
                objectBody.IsStatic = false;
            this.scale = scale;
            this.depth = depth;

            //loads fixture details
            objectFixture = FixtureFactory.AttachRectangle(
               texture.Width / PIXELS_PER_METER,
               texture.Height / PIXELS_PER_METER,
               1, Vector2.Zero, objectBody);
            objectFixture.Restitution = RESTITUTION;
            objectFixture.Friction = FRICTION;


        } //end PhysicsGameObect

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                objectBody.Position * PIXELS_PER_METER,
                null,
                Color.White,
                objectBody.Rotation * PIXELS_PER_METER,
                new Vector2(texture.Width / 2, texture.Height / 2),
                scale,
                SpriteEffects.None,
                depth);
        } //end Draw

    } //end class
}
