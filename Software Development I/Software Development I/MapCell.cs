using System.Collections.Generic;

namespace Software_Development_I
{
    class MapCell
    {
        public List<int> baseTiles = new List<int>();

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

        public MapCell(int tileID)
        {
            this.tileID = tileID;
        } //end MapCell

        public void AddBaseTile(int tileID)
        {
            baseTiles.Add(tileID);
        } //end AddBaseTile
    } //end class MapCell
} //end namespace
