using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace Software_Development_I
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private World world;
        private Texture2D backgroundTexture;
        private Random rando;
        private KeyboardState oldState;

        private float PIXELS_PER_METER;


        PhysicsGameObject floor, myBall;
        List<PhysicsGameObject> obstacles, otherBalls;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            world = new World(new Vector2(0, 9.81f));
            rando = new Random();
            oldState = Keyboard.GetState();
            PIXELS_PER_METER = 20;
            base.Initialize();
        } //end Initialize

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("Background");

            //loads floor
            floor = new PhysicsGameObject(
                Content.Load<Texture2D>("Floor"),
                world,
                true,
                new Vector2(400 / PIXELS_PER_METER, 469 / PIXELS_PER_METER));

            //loads controllable ball
            myBall = new PhysicsGameObject(
                Content.Load<Texture2D>("MyBall"),
                world,
                false,
                new Vector2(40 / PIXELS_PER_METER, 432 / PIXELS_PER_METER));

            //Loads other balls randomly
            otherBalls = new List<PhysicsGameObject>();
            for (int i = 0; i < rando.Next(1, 16); i++)
            {
                PhysicsGameObject tempPGO = new PhysicsGameObject(
                    Content.Load<Texture2D>("OtherBall"),
                    world,
                    false,
                    new Vector2(rando.Next(100, 700) / PIXELS_PER_METER, rando.Next(100, 400) / PIXELS_PER_METER));

                otherBalls.Add(tempPGO);
            } //end for

            //Loads obstacles randomly
            obstacles = new List<PhysicsGameObject>();
            for (int i = 0; i < rando.Next(1, 6); i++)
            {
                PhysicsGameObject tempPGO = new PhysicsGameObject(
                    Content.Load<Texture2D>("Obstacle"),
                    world,
                    true,
                    new Vector2(rando.Next(100, 700) / PIXELS_PER_METER, rando.Next(100, 400) / PIXELS_PER_METER));

                obstacles.Add(tempPGO);
            } //end for

        } //end LoadContent


        protected override void UnloadContent()
        {

        } //end UnloadContent

        protected override void Update(GameTime gameTime)
        {
            KeyboardState curState = Keyboard.GetState();

            if (curState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (curState.IsKeyDown(Keys.A))
                myBall.objectBody.ApplyLinearImpulse(
                    new Vector2(-5, 0) / PIXELS_PER_METER);

            if (curState.IsKeyDown(Keys.D))
                myBall.objectBody.ApplyLinearImpulse(
                    new Vector2(5, 0) / PIXELS_PER_METER);

            if (curState.IsKeyDown(Keys.Space) &&
                oldState.IsKeyUp(Keys.Space))
                myBall.objectBody.ApplyLinearImpulse(new Vector2(0, -7));

            oldState = curState;
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        } //end Update

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            //draws floor
            floor.Draw(spriteBatch);
            //draws player's ball
            myBall.Draw(spriteBatch);
            //draws obstacles
            for (int i = 0; i < obstacles.Count; i++)
                obstacles[i].Draw(spriteBatch);
            //draws other balls
            for (int i = 0; i < otherBalls.Count; i++)
                otherBalls[i].Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        } //end Draw
    } //end class
} //end namespace
