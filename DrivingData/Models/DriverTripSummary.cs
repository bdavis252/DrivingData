using System;

namespace DrivingData.Models
{
    class DriverTripSummary
    {
        public string DriverName { get; set; }
        public decimal TotalDistance { get; set; }
        public int TotalMinutes { get; set; } // I used int here because times are always given in whole minutes already

    }
}
