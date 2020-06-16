using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static population.Common;

namespace population
{
    class Program
    {
        static void Main(string[] args)
        {
            window = new RenderWindow(VideoMode.DesktopMode, "Population 1.0");

            window.Size = new Vector2u(1600, 900);
            window.Closed += Win_Close;

            grid = new Grid(180, 320, 5);
            grid.AddCell(new Entity(), 1, 1);
            /*
            grid.AddCell(new Entity(), 151, 150);
            grid.AddCell(new Entity(), 152, 150);
            grid.AddCell(new Entity(), 110, 150);
            grid.AddCell(new Entity(), 121, 150);
            grid.AddCell(new Entity(), 132, 150);
            grid.AddCell(new Entity(), 140, 150);
            grid.AddCell(new Entity(), 51, 150);
            grid.AddCell(new Entity(), 2, 150);
            */
            while (window.IsOpen)
            {
                window.DispatchEvents();

                grid.SpawnOrganics();
                grid.RunCycle();
                grid.Draw(0, 0);
                
                window.Display();
                window.Clear();
            }
        }

        private static void Win_Close(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
