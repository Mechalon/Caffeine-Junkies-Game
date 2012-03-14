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

        int baseOffsetX = -32;
        int baseOffsetY = -64;
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
            //Texture2D tiles = Content.Load<Texture2D>(@"Textures\TileSets\input1");
            //testLevel = new TileMap("testlevel", Content.RootDirectory, tiles, 32, 32, 2);
            Texture2D tiles2 = Content.Load<Texture2D>(@"Textures\TileSets\input2");
            testLevel = new TileMap("testleveltwo", Content.RootDirectory, tiles2, 48, 48, 0);

            
            /*
             * This is the code to initialize the camera.
             * ViewWidth and ViewHeight set the viewport in the camera class.
             * WorldWidth and WorldHeight set the dimensions of the level map.
             * DisplayOffset helps the camera stay in bounds.
             */
            Camera.viewWidth = this.graphics.PreferredBackBufferWidth;
            Camera.viewHeight = this.graphics.PreferredBackBufferHeight;
            Camera.worldWidth = testLevel.mapWidth * testLevel.tileProperties.width;
            Camera.worldHeight = testLevel.mapHeight * testLevel.tileProperties.height;
            Camera.displayOffset = new Vector2(baseOffsetX, baseOffsetY);
            Camera.location = Vector2.Zero;
            
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

            if (currKeyState.IsKeyDown(Keys.Left))
                Camera.Move(new Vector2(-2, 0));
            if (currKeyState.IsKeyDown(Keys.Right))
                Camera.Move(new Vector2(2, 0));
            if (currKeyState.IsKeyDown(Keys.Up))
                Camera.Move(new Vector2(0, -2));
            if (currKeyState.IsKeyDown(Keys.Down))
                Camera.Move(new Vector2(0, 2));

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
