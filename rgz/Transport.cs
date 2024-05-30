using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgz
{
    public abstract class Transport
    {
        public double ArrivalTime { get; set; }
        public int MaxCapacity { get; protected set; }
        public int OccupiedPlaces { get; set; }
    }
}
