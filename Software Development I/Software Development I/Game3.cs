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

        TileMap testLevel;

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

            /*
             * This is the code to read in levels.
             * The constructor reads in the map data file (.map) located in Content\MapData
             * that is specified by the Content.RootDirectory that is set in the Game3
             * constructor. It also reads in the tile set data and creates a Tile class
             * within the TileMap class that uses the texture that is given to, in this
             * case, tiles and defines the dimensions of each tile in the tile set texture.             * 
             */
            Texture2D tiles = Content.Load<Texture2D>(@"Textures\TileSets\part2_tileset");
            testLevel = new TileMap("testlevel", Content.RootDirectory, tiles, 48, 48);
            
            base.LoadContent();
        } //end LoadContent

        protected override void UnloadContent()
        {
            base.UnloadContent();
        } //end UnloadContent

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currKeyState = Keyboard.GetState();
            if (currKeyState.IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        } //end Update

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            testLevel.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        } //end Draw
    } //end class Game3
} //end namespace
