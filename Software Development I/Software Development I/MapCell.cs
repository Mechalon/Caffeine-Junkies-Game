using System.Collections.Generic;

namespace Software_Development_I
{
    class MapCell
    {
        public List<int> baseTiles = new List<int>();

        /// <summary>
        /// Tile number set/returned for drawing the level. Tile numbers start at top left and increase horizontally then vertically withinthe texture.
        /// </summary>
        public int tileID
        {
            get
            {
                if (baseTiles.Count > 0)
                    return baseTiles[0];
                return 0;
            } //end get

            set
            {
                if (baseTiles.Count > 0)
                    baseTiles[0] = value;
                else
                    AddBaseTile(value);
            } //end set
        } //end tileID

        /// <summary>
        /// Creates a base layer for each cell in the tile map.
        /// </summary>
        /// <param name="tileID">
        /// Tile number used for base layer.
        /// </param>
        public MapCell(int tileID)
        {
            this.tileID = tileID;
        } //end MapCell

        /// <summary>
        /// Adds a layer to the Map Cell.
        /// </summary>
        /// <param name="tileID">
        /// Tile number used for additional layer.
        /// </param>
        public void AddBaseTile(int tileID)
        {
            baseTiles.Add(tileID);
        } //end AddBaseTile
    } //end class MapCell
} //end namespace
