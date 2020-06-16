using SFML.Graphics;
namespace population
{
    abstract class Cell
    {
        public uint energy { get; set; }
        public Color color { get; protected set; }
        public TypeOfCell type { get; protected set; }
    }
}
