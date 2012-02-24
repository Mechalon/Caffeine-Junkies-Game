using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    class LevelLayer
    {
        private Texture2D tileSheet;
        private string contentPath;

        private int[,] mapData;
        public List<Rectangle> bounds;
        public int width;
        public int height;

        public LevelLayer(Texture2D tileSheet, string contentPath)
        {
            this.tileSheet = tileSheet;
            this.contentPath = contentPath;
        } //end LevelLayer

        public void OpenMap(string mapName)
        {
            string path = contentPath + @"\" + mapName + ".map";

            using (StreamReader sr = new StreamReader(path))
            {
                int curY = 0;
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    if (curY == 0)
                    {
                        string[] dimensions = line.Split(' ');
                        width = int.Parse(dimensions[0]);
                        height = int.Parse(dimensions[1]);
                        mapData = new int[width, height];

                        for (int x = 0; x < width; x++)
                            for (int y = 0; y < height; y++)
                                mapData[x, y] = 0;
                    } //end if
                    else
                    {
                        int curX = 0;
                        string[] tiles = line.Split(' ');
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            try
                            {
                                mapData[curX, curY - 1] = int.Parse(tiles[i]);
                            } //end try
                            catch (Exception) { }
                            curX++;
                        } //end for
                    } //end else
                    curY++;
                } //end while
            } //end using

            AddBounds();
        } //end OpenMap

        private void AddBounds()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (mapData[x, y] != 0)
                    {
                        Rectangle boundary;

                        switch (mapData[x, y] - 1)
                        {
                            case 1:
                                break;
                            case 2:
                                break;
                        } //end switch
                    } //end if
                } //end for
        } //end AddBounds

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (mapData[x, y] != 0)
                    {
                        int sourceY = 34 * (mapData[x, y] - 1);
                        Rectangle sourceRect = new Rectangle(0, sourceY, 32, 32);
                        Rectangle destinRect = new Rectangle(x * 32, y * 32, 32, 32);
                        spriteBatch.Draw(tileSheet, destinRect, sourceRect, Color.White);
                    } //end if
                } //end for
        } //end Draw
    } //end class LevelLayer
} //end namespaces
