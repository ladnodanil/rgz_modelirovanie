using System;
using System.Collections.Generic;
using System.Linq;

namespace rgz
{
    public class BusStop
    {
        public int SimulationTime { get; set; }

        const double mean = 1;
        const double stddev = 0.87;

        private double meanBus;
        private double meanMinibus;

        Random random = new Random();

        private List<Passenger> passengers;
        private Queue<Bus> buses;
        private Queue<Minibus> minibuses;

        private double T_ozh = 0;
        private int N = 0;

        private double totalQueueLength = 0;
        private int queueUpdateCount = 0;

        private List<double> busLoadFactors;
        private List<double> minibusLoadFactors;

        public BusStop(int simulationTime, int numberOfBuses, int numberOfMinibuses)
        {
            buses = new Queue<Bus>();
            minibuses = new Queue<Minibus>();
            passengers = new List<Passenger>();
            SimulationTime = simulationTime;
            meanBus = simulationTime / numberOfBuses;
            meanMinibus = simulationTime / numberOfMinibuses;

            busLoadFactors = new List<double>();
            minibusLoadFactors = new List<double>();
        }

        public (double, double, double, double) Simulate()
        {
            SimulatePassenger();
            SimulateBus();
            SimulateMinibus();
            
            double avgWaitTime = T_ozh / N;
            double avgQueueLength = totalQueueLength / queueUpdateCount;

            
            Console.WriteLine($"Среднее время ожидания: {avgWaitTime:F1} минут");
            Console.WriteLine($"Средняя длина очереди: {avgQueueLength:F1}");
            Console.WriteLine($"Осталось человек на остановке: {passengers.Count(p => p.HasNotLeft)}");

            return (avgWaitTime, avgQueueLength, busLoadFactors.Average(), minibusLoadFactors.Average());
        }
        public void SimulateBus()
        {

            double t_prix_bus = 0;
            while (t_prix_bus < SimulationTime)
            {
                t_prix_bus += NormalDistribution(meanBus, 2, random);
                if (t_prix_bus < SimulationTime)
                {
                    Bus bus = new Bus();
                    bus.ArrivalTime = t_prix_bus;

                    buses.Enqueue(bus);
                    BoardPassengers(t_prix_bus, TransportPreference.Bus);
                }
            }
        }

        public void SimulateMinibus()
        {

            double t_prix_minibus = 0;
            while (t_prix_minibus < SimulationTime)
            {
                t_prix_minibus += NormalDistribution(meanMinibus, 1.5, random);
                if (t_prix_minibus < SimulationTime)
                {
                    Minibus minibus = new Minibus();
                    minibus.ArrivalTime = t_prix_minibus;

                    minibuses.Enqueue(minibus);
                    BoardPassengers(t_prix_minibus, TransportPreference.Minibus);
                }
            }
        }

        private void BoardPassengers(double currentTime, TransportPreference transportType)
        {
            if ((transportType == TransportPreference.Bus && buses.Count > 0) ||
                (transportType == TransportPreference.Minibus && minibuses.Count > 0))
            {
                Transport transport = transportType == TransportPreference.Bus ? (Transport)buses.Dequeue() : minibuses.Dequeue();
                int remainingCapacity = transport.MaxCapacity - transport.OccupiedPlaces;
                int boardedPassengers = 0;
                int passengersInQueue = 0;

                foreach (var passenger in passengers)
                {
                    if (passenger.ArrivalTime <= currentTime && passenger.HasNotLeft &&
                        (passenger.Preference == transportType || passenger.Preference == TransportPreference.Both))
                    {
                        passengersInQueue++;
                    }
                    else if (passenger.ArrivalTime > currentTime)
                    {
                        break;
                    }
                }

                totalQueueLength += passengersInQueue;
                queueUpdateCount++;

                foreach (var passenger in passengers)
                {
                    if (passenger.ArrivalTime <= currentTime && passenger.HasNotLeft &&
                        (passenger.Preference == transportType || passenger.Preference == TransportPreference.Both) &&
                        remainingCapacity > 0)
                    {
                        Console.WriteLine($"Пассажир, пришедший в {ConvertMinutesToTimeString(passenger.ArrivalTime)}, ожидает {transport.ArrivalTime - passenger.ArrivalTime:F1} минут");
                        T_ozh += (transport.ArrivalTime - passenger.ArrivalTime);
                        N++;
                        boardedPassengers++;
                        remainingCapacity--;
                        passenger.HasNotLeft = false;
                    }
                }

                double loadFactorBefore = (double)transport.OccupiedPlaces / transport.MaxCapacity;
                transport.OccupiedPlaces += boardedPassengers;
                double loadFactorAfter = (double)transport.OccupiedPlaces / transport.MaxCapacity;

                if (transportType == TransportPreference.Bus)
                {
                    busLoadFactors.Add(loadFactorAfter);
                }
                else
                {
                    minibusLoadFactors.Add(loadFactorAfter);
                }

                Console.WriteLine($"{(transportType == TransportPreference.Bus ? "Автобус" : "Маршрутка")} прибыл {ConvertMinutesToTimeString(currentTime)}, забрал {boardedPassengers} пассажиров.");
                Console.WriteLine($"Загруженность до: {loadFactorBefore:F2}");
                Console.WriteLine($"Загруженность после: {loadFactorAfter:F2}");
                Console.WriteLine($"Очередь: {passengersInQueue}\n");
            }
        }

        public void SimulatePassenger()
        {
            double t_prix = 0;
            while (t_prix < SimulationTime)
            {
                t_prix += NormalDistribution(mean, stddev, random);
                if (t_prix < SimulationTime)
                {
                    Passenger passenger = new Passenger();
                    passenger.ArrivalTime = t_prix;
                    passenger.Preference = (TransportPreference)random.Next(0, 2);
                    passenger.HasNotLeft = true;
                    passengers.Add(passenger);
                }
            }
        }

        private string ConvertMinutesToTimeString(double minutes)
        {
            int totalSeconds = (int)(minutes * 60);
            int hours = 12 + totalSeconds / 3600;
            int mins = (totalSeconds % 3600) / 60;
            int secs = totalSeconds % 60;
            return $"{hours:D2}:{mins:D2}:{secs:D2}";
        }

        public static double NormalDistribution(double mean, double stddev, Random random)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stddev * randStdNormal;
        }
    }
}