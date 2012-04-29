#region
/*
 * TileMap.cs
 * Created by Forrest
 */
#endregion
using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    class TileMap
    {
        public Texture2D tileSheet;
        public List<MapRow> rows = new List<MapRow>();
        public int mapWidth;
        public int mapHeight;



        /// <summary>
        /// Creates a level using Tiles and Map Cells.
        /// </summary>
        /// <param name="levelPath">
        /// Path of the file containing tile placement within the level.
        /// </param>
        /// <param name="tileTexture">
        /// Tile sheet texture used to draw the level.
        /// </param>
        /// <param name="tileWidth">
        /// //Width of each tile in texture.
        /// </param>
        /// <param name="tileHeight">
        /// //Height of each tile in texture.
        /// </param>
        /// <param name="spacing">
        /// //Spacing between each tile in texture.
        /// </param>
        public TileMap(Level level, int levelIndex, Texture2D tileSheet)
        {
            this.tileSheet = tileSheet;

            string levelPath = string.Format("Content/Levels/{0}.map", levelIndex);

            using (StreamReader sr = new StreamReader(levelPath))
            {
                int curY = 0;
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    //gets map dimensions and draws a blank map on first read
                    if (curY == 0)
                    {
                        string[] dimensions = line.Split(',');
                        mapWidth = int.Parse(dimensions[0]);
                        mapHeight = int.Parse(dimensions[1]);
                        Camera.worldWidth = mapWidth * Tile.WIDTH;
                        Camera.worldHeight = mapHeight * Tile.HEIGHT;

                        for (int y = 0; y < mapHeight; y++)
                        {
                            MapRow newRow = new MapRow();
                            for (int x = 0; x < mapWidth; x++)
                            {
                                newRow.columns.Add(new Tile(0, TileCollision.Passable));
                            } //end for
                            rows.Add(newRow);
                        } //end for
                    } //end if
                    else
                    {

                        //second line contains base layer info
                        if (curY >= 1 && curY <= mapHeight)
                        {
                            int curX = 0;
                            string[] tiles = line.Split(',');

                            //reads line in as tiles
                            for (int i = 0; i < tiles.Length; i++)
                            {
                                try
                                {
                                    int check = int.Parse(tiles[i]);
                                    if (check != 0)
                                    {
                                        rows[curY - 1].columns[i].tileID = check;
                                        rows[curY - 1].columns[i].collision = TileCollision.Impassable;
                                    } //end if
                                } //end try
                                catch (Exception)
                                {
                                    try
                                    {
                                        char check = tiles[i][0];

                                        switch (check)
                                        {
                                            case 'S':
                                                rows[curY - 1].columns[i].tileID = 0;
                                                rows[curY - 1].columns[i].collision = TileCollision.Passable;
                                                level.SetStart(curX, curY - 1);
                                                break;
                                            case 'C':
                                                rows[curY - 1].columns[i].tileID = 3;
                                                rows[curY - 1].columns[i].collision = TileCollision.Passable;
                                                level.SetCheck(curX, curY - 1);
                                                break;
                                            case 'X':
                                                rows[curY - 1].columns[i].tileID = 3;
                                                rows[curY - 1].columns[i].collision = TileCollision.Passable;
                                                level.SetEnd(curX, curY - 1);
                                                break;
                                            default:
                                                break;
                                        } //end switch
                                    } //end try
                                    catch (Exception) { }

                                } //end catch
                                curX++;
                            } //end for
                        } //end if
                    } //end else
                    curY++;
                } //end while
            } //end using
        } //end TileMap

        /// <summary>
        /// Draws the Tile Map.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            int firstX = (int)(Camera.Location.X / Tile.WIDTH);
            int firstY = (int)(Camera.Location.Y / Tile.HEIGHT);

            int offsetX = (int)(Camera.Location.X % Tile.WIDTH);
            int offsetY = (int)(Camera.Location.Y % Tile.HEIGHT);

            for (int y = 0; y < Camera.viewHeight / Tile.HEIGHT + 1; y++)
                for (int x = 0; x < Camera.viewWidth / Tile.WIDTH + 1; x++)

                    if (rows[firstY + y].columns[firstX + x].tileID > 0)
                        spriteBatch.Draw(
                            tileSheet,
                            new Rectangle(
                                x * Tile.WIDTH - offsetX,
                                y * Tile.HEIGHT - offsetY,
                                Tile.WIDTH,
                                Tile.HEIGHT),
                            rows[firstY + y].columns[firstX + x].GetSourceRectangle(tileSheet),
                            Color.White);

        } //end Draw
    } //end class TileMap

    /// <summary>
    /// Used for creating a multi-dimensional list.
    /// </summary>
    class MapRow
    {
        public List<Tile> columns = new List<Tile>();
    } //end class MapRow
} //end namespace
