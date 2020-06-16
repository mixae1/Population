using SFML.Graphics;
using System;
using static population.Common;

namespace population
{
    class Entity : Cell
    {
        public Gen gen { get; }
        private int posInGen;

        public Entity()
        {
            energy = (uint)rand.Next(30);
            color = new Color(RandByte(), RandByte(), RandByte());
            gen = new Gen(40);
            gen.LoadRandom();
            type = TypeOfCell.Entity;
            posInGen = 0;
        }
        public Entity(byte value)
        {
            energy = (uint)rand.Next(100);
            color = new Color(RandByte(), RandByte(), RandByte());
            gen = new Gen(40);
            gen.LoadValue(value);
            type = TypeOfCell.Entity;
            posInGen = 0;
        }
        public Entity(Entity ancestor)
        {
            energy = 5;
            type = TypeOfCell.Entity;
            //25%
            bool buf = rand.Next(20) == 0;
            gen = (buf ? ancestor.gen.GetDuplicate(true) : ancestor.gen);
            color = (buf ? new Color(RandByte(), RandByte(), RandByte()) : ancestor.color);
            posInGen = 0;
        }

        public void Run()
        {
            byte steps = 0;
            //Console.WriteLine(posInGen);
            while(steps++ < 10)
            {
                posInGen = (posInGen + gen.capas) % gen.capas;
                switch (gen[posInGen])
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        if (grid.Move(pos[gen[posInGen]])) posInGen++;
                        else posInGen += 2;
                        return;
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                        if (grid.Catch(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        return;
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                        if (grid.Destroy(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        return;
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                        if (grid.IfOrganic(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        break;
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        if (grid.IfCorpse(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        break;
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                    case 46:
                    case 47:
                        if (grid.IfEntity(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        break;
                    case 48:
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                        if (grid.IfNone(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        break;
                    case 56:
                    case 57:
                    case 58:
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                        if (grid.GiveAway(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        return;
                    case 64:
                    case 65:
                    case 66:
                    case 67:
                    case 68:
                    case 69:
                    case 70:
                    case 71:
                        if (grid.Produce(pos[gen[posInGen] % 8])) posInGen++;
                        else posInGen += 2;
                        return;
                    case 72:
                    case 73:
                    case 74:
                    case 75:
                    case 76:
                    case 77:
                    case 78:
                    case 79:
                        if (energy > 5 + gen[posInGen] % 8) posInGen++;
                        else posInGen += 2;
                        break;
                    case 80:
                        if(energy < 90) energy++;
                        posInGen++;
                        return;
                    case 81:
                        posInGen++;
                        return;
                }
            }
        }
    }
}
