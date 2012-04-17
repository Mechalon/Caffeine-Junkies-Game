using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

/*
 * Revise UpdateLastCheckPoint
 */
namespace Software_Development_I
{
    class CheckPoint
    {
        //list of all checkpoints in game/level
        public static List<CheckPoint> list = new List<CheckPoint> { };

        //checkpoint to return on death
        public static CheckPoint last;


        //values to return to after death
        private int level;
        private int xPos;
        private int yPos;

        public CheckPoint(int level, int xPos, int yPos)
        {
            this.level = level;
            this.xPos = xPos;
            this.yPos = yPos;
            list.Add(this);
        } //end CheckPoint

        public static void UpdateLastCheckPoint(int currentLevel)
        {
            for (int i = 0; i < list.Count(); i++)
                if (list[i].level == currentLevel && list[i].xPos < Player.position.x < list[i].xPos + 5)
                {
                    last = list[i];
                    Settings.Save();
                };
        } //end UpdateLastCheckPoint

    } //end CheckPoint
} //end
