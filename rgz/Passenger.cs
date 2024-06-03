using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rgz
{
    public enum TransportPreference
    {
        Bus,
        Minibus,
        Both
    }
    public class Passenger
    {
        public double ArrivalTime { get; set; }

        public TransportPreference Preference { get; set; }

        public bool HasNotLeft { get; set; }
       
    }
}
