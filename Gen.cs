using System;
using static population.Common;
namespace population
{
    class Gen
    {
        private byte[] gen;
        public int capas { get; } = 82;

        public Gen(int size)
        {
            gen = new byte[size];
        }

        private void Normalize(ref int index)
        {
            index = (index + gen.Length) % gen.Length;
        }
        public byte this[int index]
        {
            get 
            {
                Normalize(ref index);
                return gen[index]; 
            }
        }
        public void LoadValue(byte value)
        {
            for (int i = 0; i < gen.Length; i++) gen[i] = value;
        }
        public void LoadRandom()
        {
            for (int i = 0; i < gen.Length; i++) gen[i] = RandByte(capas);
        }
        public Gen GetDuplicate(bool mutation)
        {
            Gen newGen = new Gen(gen.Length);
            if(!mutation)
            {
                for (int i = 0; i < gen.Length; i++) newGen.gen[i] = gen[i]; 
            }
            else
            {
                int where = rand.Next(gen.Length);
                newGen.gen[where] = RandByte(capas);

                for (int i = 0; i < where; i++) newGen.gen[i] = gen[i];
                for (int i = where + 1; i < gen.Length; i++) newGen.gen[i] = gen[i];
            }
            return newGen;
        }
    }
}
