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
    }
}
