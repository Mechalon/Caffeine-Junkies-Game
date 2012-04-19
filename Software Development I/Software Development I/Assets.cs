using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Software_Development_I
{
    static class Assets
    {
        //TileMap .map Assets
        public static string testMap;

        //TileSet .png Assets
        public static Texture2D testSet;

        public static void Load(ContentManager Content)
        {
            testSet = Content.Load<Texture2D>(@"Textures\TileSets\" + "red1");
        } //end Load
    } //end Assets
} //end
