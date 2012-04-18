using System;
using System.IO;

namespace Software_Development_I
{
    static class Settings
    {
        //setting saves
        public const string file = ".saveGame";
        public static int masterVolume = 100;
        public static int musicVolume = 100;
        public static int sfxVolume = 100;

        //level saves
        public static int levelAchieved = 1;
        //checkpoint data
        public static CheckPoint checkPoint;

        //player saves
        public static int lives = 3;
        public static int ammo = 0;
        
        public static void Load()
        {
            StreamReader read = null;
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (read = new StreamReader(file))
                {
                    String line;

                    //read and load sound
                    line = read.ReadLine();
                    masterVolume = Convert.ToInt32(line);
                    line = read.ReadLine();
                    musicVolume = Convert.ToInt32(line);
                    line = read.ReadLine();
                    sfxVolume = Convert.ToInt32(line);

                    //read and load level
                    line = read.ReadLine();
                    levelAchieved = Convert.ToInt32(line);

                    //read and load player
                    line = read.ReadLine();
                    lives = Convert.ToInt32(line);
                    line = read.ReadLine();
                    ammo = Convert.ToInt32(line);
                } //end using
            } //end try
            catch (Exception e) { }
        } //end Load

        public static void Save()
        {
            StreamWriter write = null;
            try
            {
                using (write = new StreamWriter(@file))
                {
                    //write and save sound
                    write.WriteLine(masterVolume.ToString());
                    write.WriteLine(musicVolume.ToString());
                    write.WriteLine(sfxVolume.ToString());

                    //write and save level 
                    write.WriteLine(levelAchieved.ToString());

                    //write and save player
                    write.WriteLine(lives.ToString());
                    write.WriteLine(ammo.ToString());

                } //end using
            } //end try
            catch (Exception e) { }
        } //end Save

    } //end Settings
} //end
