using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_asynkront_trådag_BILAR
{
    internal class Car
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int Speed { get; set; }
        public int Tank { get; set; }
        public int DistanceCovered { get; set; }
        public int TimesFueled { get; set; }
        public int TimesTires { get; set; }
        public int TimesMalfunction { get; set; }
        public int TimesCrashed { get; set; }

        public static int coverDistance(Car car)
        {
            return car.DistanceCovered += car.Speed;
        }
    }
}
