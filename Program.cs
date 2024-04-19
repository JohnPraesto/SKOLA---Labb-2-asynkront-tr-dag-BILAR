namespace Labb_2_asynkront_trådag_BILAR
{
    internal class Program
    {
        static ManualResetEvent racePaused = new ManualResetEvent(true); // Initially, race is paused
        static Car carOne;
        static Car carTwo;
        static void Main(string[] args)
        {
            carOne = new Car { CarId = 1, CarName = "Saab", Speed = 100, Tank = 13, TimesFueled = 0, TimesTires = 0, TimesMalfunction = 0, TimesCrashed = 0, DistanceCovered = 0};
            carTwo = new Car { CarId = 2, CarName = "Volvo", Speed = 110, Tank = 9, TimesFueled = 0, TimesTires = 0, TimesMalfunction = 0, TimesCrashed = 0, DistanceCovered = 0 };

            List<Car> placement = new List<Car>();

            Thread carOneThread = new Thread(() =>
            {
                Go(carOne, placement);
            });

            Thread carTwoThread = new Thread(() =>
            {
                Go(carTwo, placement);
            });

            carOneThread.Start();
            carTwoThread.Start();

            // Start asynchronous task to listen for Enter key press
            ThreadPool.QueueUserWorkItem(ListenForEnterKeyPress);

            carOneThread.Join(); // Wait for carOneThread to finish
            carTwoThread.Join(); // Wait for carTwoThread to finish

            Console.WriteLine("\nRACE IS FINISHED! Press Enter to see results.");
            Console.ReadLine();

            Console.WriteLine("\n------ PLACEMENTS ------");

            int i = 1;
            foreach (var item in placement)
            {
                Console.WriteLine($"{i}. {item.CarName}");
                i++;
            }

        }

        public static void Go(Car car, List<Car> placement)
        {
            int maxFuel = car.Tank;
            Console.WriteLine($"{car.CarName} started!");
            while (true)
            {   
                racePaused.WaitOne(); // Wait for signal to continue race

                car.DistanceCovered = Car.coverDistance(car);
                Console.WriteLine($"{car.CarName} distance left: {10000 - car.DistanceCovered}");
                if (car.DistanceCovered >= 10000)
                {
                    Console.WriteLine($"    {car.CarName} HAS REACHED THE FINISH LINE!");
                    placement.Add(car);
                    break;
                }
                car.Tank -= 1;
                if (car.Tank == 0)
                {
                    car.Tank = maxFuel;
                    car.TimesFueled += 1;
                    Console.WriteLine($"    {car.CarName} must stop to re-fuel!");
                    Thread.Sleep(1603);
                }
                Thread.Sleep(3000);
                Accident(car);
            }
        }

        public static void Accident(Car car)
        {
            Random random = new Random();
            
            switch(random.Next(1, 50))
            {
                case 1:
                case 2:
                    Console.WriteLine($"    The tires of the {car.CarName} explodes! Changing new tires.");
                    car.TimesTires += 1;
                    Thread.Sleep(1950);
                    break;

                case 3:
                case 4:
                    car.Speed -= 3;
                    car.TimesMalfunction += 1;
                    Console.WriteLine($"    The engine of {car.CarName} malfunctions! Speed reduced to {car.Speed}.");
                    break;

                case 5:
                case 6:
                    Console.WriteLine($"    {car.CarName} takes a wrong turn! Lost 300 distance to catch up!");
                    car.DistanceCovered -= 300;
                    break;

                case 7:
                    Console.WriteLine($"    The {car.CarName} CRASHES into a tree. Must wait for a new car.");
                    Thread.Sleep(7700);
                    car.Speed += 20;
                    car.TimesCrashed += 1;
                    Console.WriteLine($"    {car.CarName} is BACK! This time with a new ELECTRIC motor! New speed {car.Speed}");
                    break;

                default:
                    break;
            }
        }

        public static void ListenForEnterKeyPress(object state)
        {
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    racePaused.Reset(); // Pause the race
                    Console.WriteLine("-------------------------");
                    Console.WriteLine($"               {carOne.CarName}   {carTwo.CarName}");
                    Console.WriteLine($"Speed:         {carOne.Speed, -7}{carTwo.Speed}");
                    Console.WriteLine($"Re-fueled:     {carOne.TimesFueled,-7} {carTwo.TimesFueled}");
                    Console.WriteLine($"Changed tires: {carOne.TimesTires,-7} {carTwo.TimesTires}");
                    Console.WriteLine($"Malfunctions:  {carOne.TimesMalfunction,-7} {carTwo.TimesMalfunction}");
                    Console.WriteLine($"Crashed:       {carOne.TimesCrashed,-7} {carTwo.TimesCrashed}");
                    Console.WriteLine($"Distance:      {carOne.DistanceCovered,-7} {carTwo.DistanceCovered}");
                    Console.WriteLine("Press Enter to resume race...");
                    Console.WriteLine("-------------------------");
                    Console.ReadLine();
                    racePaused.Set(); // Resume the race
                }

                Thread.Sleep(100);
            }
        }
    }
}
