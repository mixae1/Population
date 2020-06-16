using SFML.Graphics;
namespace population
{
    class Corpse : Cell
    {
        public Corpse(uint energy)
        {
            this.energy = energy;
            color = Color.Black;
            type = TypeOfCell.Corpse;
        }
    }
}
