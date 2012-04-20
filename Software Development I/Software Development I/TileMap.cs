using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    class TileMap
    {
        public Tile tileProperties;
        public List<MapRow> rows = new List<MapRow>();
        public int mapWidth;
        public int mapHeight;
        int squaresHori = 16; //number of squares to display horizontally
        int squaresVert = 8; //number of squares to display vertically

        public TileMap(string levelPath, Texture2D tileTexture, int tileWidth, int tileHeight, int spacing)
        {
            tileProperties = new Tile(tileTexture, tileWidth, tileHeight, spacing);

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
                        Camera.worldWidth = mapWidth * tileWidth;
                        Camera.worldHeight = mapHeight * tileHeight;

                        for (int y = 0; y < mapHeight; y++)
                        {
                            MapRow newRow = new MapRow();
                            for (int x = 0; x < mapWidth; x++)
                            {
                                newRow.columns.Add(new MapCell(0));
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
                                        rows[curY-1].columns[i].tileID = check;
                                } //end try
                                catch (Exception) { }
                                curX++;
                            } //end for
                        } //end if

                        //other lines contain additional layer info
                        else
                        {
                            string[] tiles = line.Split(',');
                            try
                            {
                                int colValue = int.Parse(tiles[0]);
                                int rowValue = int.Parse(tiles[1]);
                                int tileValue = int.Parse(tiles[2]);
                                rows[rowValue].columns[colValue].AddBaseTile(tileValue);
                            } //end try
                            catch (Exception) { }
                        } //end else
                    } //end else
                    curY++;
                } //end while
            } //end using
        } //end TileMap

        public void Draw(SpriteBatch spriteBatch)
        {
            int firstX = (int)(Camera.Location.X / tileProperties.width);
            int firstY = (int)(Camera.Location.Y / tileProperties.height);

            int offsetX = (int)(Camera.Location.X % tileProperties.width);
            int offsetY = (int)(Camera.Location.Y % tileProperties.height);

            for (int y = 0; y < Camera.viewHeight / tileProperties.height + 1; y++)
                for (int x = 0; x < Camera.viewWidth / tileProperties.width + 1; x++)
                    foreach (int tileID in rows[firstY + y].columns[firstX + x].baseTiles)
                    {
                        if (tileID > 0)
                            spriteBatch.Draw(
                                tileProperties.tileSetTexture,
                                new Rectangle(
                                    x * tileProperties.width - offsetX,
                                    y * tileProperties.height - offsetY,
                                    tileProperties.width,
                                    tileProperties.height),
                                tileProperties.GetSourceRectangle(tileID-1),
                                Color.White);
                    } //end for
        } //end Draw
    } //end class TileMap

    class MapRow
    {
        public List<MapCell> columns = new List<MapCell>();
    } //end class MapRow
} //end namespace
