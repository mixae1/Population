using SFML.Graphics;
namespace population
{
    class Organics : Cell
    {
        public Organics()
        {
            color = Color.Green;
            energy = 1;
            type = TypeOfCell.Organics;
        }
    }
}
