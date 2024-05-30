using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgz
{
    public class Minibus : Transport
    {
        Random random = new Random();

        public Minibus() 
        {
            MaxCapacity = 15;
            OccupiedPlaces = random.Next(0, MaxCapacity);
        }
    }
}
