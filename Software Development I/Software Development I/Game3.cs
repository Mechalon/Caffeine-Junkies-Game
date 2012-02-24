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

namespace Software_Development_I
{
    class Game3 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private LevelLayer interactiveLayer;

        public Game3()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        } //end Game3

        protected override void Initialize()
        {
            base.Initialize();
        } //end Initialize

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            interactiveLayer = new LevelLayer(
                Content.Load<Texture2D>("input1"),
                Content.RootDirectory);
            interactiveLayer.OpenMap("testlevel");

            base.LoadContent();
        } //end LoadContent

        protected override void UnloadContent()
        {
            base.UnloadContent();
        } //end UnloadContent

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        } //end Update

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            interactiveLayer.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        } //end Draw
    } //end class Game3
} //end namespace
