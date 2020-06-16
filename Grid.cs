using SFML.Graphics;
using SFML.System;
using static population.Common;
namespace population
{
    class Grid
    {
        public int maxOrgs { get; }

        public int height { get; }
        public int width { get; }

        private int cntOrgs;
        private Cell[,] grid;
        private (int y, int x) curCell;
        private RectangleShape[,] array;

        public Grid(int height, int width, uint cellSize)
        {
            grid = new Cell[height, width];
            (this.height, this.width) = (height, width);

            array = new RectangleShape[height, width];
            for (uint j = 0; j < height; j++)
            {
                for (uint i = 0; i < width; i++)
                {
                    array[j, i] = new RectangleShape(ToF(new Vector2u(cellSize, cellSize)));
                    array[j, i].Position = ToF(new Vector2u(i * cellSize, j * cellSize));
                }
            }

            maxOrgs = width * height / 100;
            cntOrgs = 0;
        }

        private void Normalize(ref int y, ref int x)
        {
            (y, x) = ((y + height) % height, (x + width) % width);
        }
        private (int y, int x) Normalize(int y, int x)
        {
            return ((y + height) % height, (x + width) % width);
        }

        public void Draw(int Y, int X)
        {
            for (uint j = 0; j < height; j++)
            {
                for (uint i = 0; i < width; i++)
                {
                    switch (grid[j, i]?.type)
                    {
                        case null:
                            array[j, i].FillColor = Color.White;
                            break;
                        case TypeOfCell.None:
                            array[j, i].FillColor = Color.White;
                            break;
                        case TypeOfCell.Organics:
                        case TypeOfCell.Entity:
                        case TypeOfCell.Corpse: array[j, i].FillColor = grid[j, i].color; break;
                    }
                    window.Draw(array[j, i]);
                }
            }

        }
        public void AddCell(Cell cell, int y, int x)
        {
            Normalize(ref y, ref x);
            grid[y, x] = cell;
        }
        public void RunCycle()
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    curCell = (j, i);
                    if (grid[j, i]?.type == TypeOfCell.Entity) (grid[j, i] as Entity).Run();
                }
            }
        }
        public void SpawnOrganics()
        {
            int buf = maxOrgs - cntOrgs;
            for (int i = 0; i < buf; i++)
            {
                int y = rand.Next(height),
                    x = rand.Next(width);
                if (grid[y, x] == null)
                {
                    grid[y, x] = new Organics();
                    cntOrgs++;
                }
            }
        }

        public bool Move((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            switch (grid[offset.y, offset.x]?.type)
            {
                case null:
                    grid[offset.y, offset.x] = grid[curCell.y, curCell.x];
                    grid[curCell.y, curCell.x] = null;
                    return true;
                case TypeOfCell.Entity:
                    return false;
                case TypeOfCell.Organics:
                    grid[curCell.y, curCell.x].energy += grid[offset.y, offset.x].energy;
                    grid[offset.y, offset.x] = grid[curCell.y, curCell.x];
                    grid[curCell.y, curCell.x] = null;
                    cntOrgs--;
                    return true;
                case TypeOfCell.Corpse:
                    grid[curCell.y, curCell.x].energy += grid[offset.y, offset.x].energy;
                    grid[offset.y, offset.x] = grid[curCell.y, curCell.x];
                    grid[curCell.y, curCell.x] = null;
                    return true;
                default: return true;
            }
        }
        public bool Catch((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            switch (grid[offset.y, offset.x]?.type)
            {
                case null:
                    return true;
                case TypeOfCell.Entity:
                    return false;
                case TypeOfCell.Organics:
                    grid[curCell.y, curCell.x].energy += grid[offset.y, offset.x].energy;
                    grid[offset.y, offset.x] = null;
                    cntOrgs--;
                    return true;
                case TypeOfCell.Corpse:
                    grid[curCell.y, curCell.x].energy += grid[offset.y, offset.x].energy;
                    grid[offset.y, offset.x] = null;
                    return true;
                default: return true;
            }
        }
        public bool Destroy((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            switch (grid[offset.y, offset.x]?.type)
            {
                case null:
                    return false;
                case TypeOfCell.Entity:
                    if ((grid[offset.y, offset.x] as Entity).gen != (grid[curCell.y, curCell.x] as Entity).gen)
                    {
                        grid[offset.y, offset.x] = new Corpse(grid[offset.y, offset.x].energy);
                        return true;
                    }
                    else return false;
                case TypeOfCell.Organics:
                    grid[offset.y, offset.x] = null;
                    cntOrgs--;
                    return true;
                case TypeOfCell.Corpse:
                    grid[offset.y, offset.x] = null;
                    return true;
                default: return true;
            }
        }
        public bool IfOrganic((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            if (grid[offset.y, offset.x]?.type == TypeOfCell.Organics) return true;
            else return false;
        }
        public bool IfCorpse((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            if (grid[offset.y, offset.x]?.type == TypeOfCell.Corpse) return true;
            else return false;
        }
        public bool IfEntity((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            if (grid[offset.y, offset.x]?.type == TypeOfCell.Entity) return true;
            else return false;
        }
        public bool IfNone((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            if (grid[offset.y, offset.x]?.type == TypeOfCell.None || grid[offset.y, offset.x] == null) return true;
            else return false;
        }
        public bool GiveAway((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            if (grid[offset.y, offset.x]?.type == TypeOfCell.Entity &&
                (grid[offset.y, offset.x] as Entity).gen == (grid[curCell.y, curCell.x] as Entity).gen)
            {
                grid[offset.y, offset.x].energy++;
                if (--grid[curCell.y, curCell.x].energy <= 0) grid[curCell.y, curCell.x] = new Corpse(0);
                return true;
            }
            else return false;
        }
        public bool Produce((int y, int x) offset)
        {
            offset = Normalize(offset.y + curCell.y, offset.x + curCell.x);
            uint buf = 0;
            if (grid[curCell.y, curCell.x].energy < 5)
                grid[curCell.y, curCell.x] = new Corpse(grid[curCell.y, curCell.x].energy);
            if (grid[curCell.y, curCell.x] is Entity)
                switch (grid[offset.y, offset.x]?.type)
                {
                    case null:
                        grid[offset.y, offset.x] = new Entity(grid[curCell.y, curCell.x] as Entity);
                        grid[curCell.y, curCell.x].energy -= 5;
                        return true;
                    case TypeOfCell.Entity:
                        return false;
                    case TypeOfCell.Organics:
                        buf = grid[offset.y, offset.x].energy;
                        grid[offset.y, offset.x] = new Entity(grid[curCell.y, curCell.x] as Entity);
                        grid[offset.y, offset.x].energy += buf;
                        grid[curCell.y, curCell.x].energy -= 5;
                        cntOrgs--;
                        return true;
                    case TypeOfCell.Corpse:
                        buf = grid[offset.y, offset.x].energy;
                        grid[offset.y, offset.x] = new Entity(grid[curCell.y, curCell.x] as Entity);
                        grid[offset.y, offset.x].energy += buf;
                        grid[curCell.y, curCell.x].energy -= 5;
                        return true;
                }
            return false;
        }
    }
}
