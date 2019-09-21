using System;

namespace DrivingData.Models
{
    class Trip
    {
        public string DriverName { get; set; } // this would be an int/guid FK to Driver to ensure uniqueness
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal MilesDriven { get; set; }
    }
}
