using System;

namespace DrivingData.Models
{
    public class Trip
    {
        public Trip(string driverName, DateTime startTime, DateTime endTime, decimal milesDriven)
        {
            DriverName = driverName;
            StartTime = startTime;
            EndTime = endTime;
            MilesDriven = milesDriven;
        }
        public Trip(Driver d, DateTime startTime, DateTime endTime, decimal milesDriven)
        {
            DriverName = d.Name;
            StartTime = startTime;
            EndTime = endTime;
            MilesDriven = milesDriven;
        }

        public string DriverName { get; set; } // this would be an int/guid FK to Driver to ensure uniqueness
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal MilesDriven { get; set; }

        public int GetMinutesElapsed()
        {
            return Convert.ToInt32(EndTime.Subtract(StartTime).TotalMinutes);
        }

        public decimal GetMph()
        {
            var elapsed = GetMinutesElapsed();
            //dividing by 0 is cool, but...
            if (elapsed == 0)
            {
                return 0;
            }
            else
            {
                return MilesDriven / (elapsed / 60M);
            }
        }

        // Problem specified "to nearest int" so I'm using Convert.ToInt32 because it rounds and converts double to int in one step.
        // If this were real life, you better believe I'd be asking someone who cared which way to round these.
        // For now, "If the problem statement doesn't specify something, you can make any decision that you want."
        // Manual says:
        // "rounded to the nearest 32-bit signed integer. If value is halfway between two whole numbers, 
        // the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6."
        public int GetRoundedMph()
        {
            return Convert.ToInt32(GetMph());
        }
    }
}
