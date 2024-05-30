using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgz
{
    public class Bus : Transport
    {
        Random random = new Random();
        public Bus()
        {
            MaxCapacity = 40;
            OccupiedPlaces = random.Next(0, MaxCapacity);
        }
    }
}
