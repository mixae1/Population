using System;
using SFML.Graphics;
using SFML.System;
namespace population
{
    static class Common
    {
        public static RenderWindow window;
        public static Grid grid;

        public static (int offset_y, int offset_x)[] pos = new (int, int)[] { (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1) };

        public static Random rand = new Random(Environment.TickCount);
        public static byte RandByte(int max = 256) => (byte)rand.Next(max);

        public static Vector2f ToF(Vector2i i) => window.MapPixelToCoords(i);
        public static Vector2f ToF(Vector2u u) => window.MapPixelToCoords(new Vector2i((int)u.X, (int)u.Y));

    }
}
