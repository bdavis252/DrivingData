using DrivingData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DrivingData
{
    // So this isn't a real web API but if it were these would be service endpoints
    public class UserDataCollectionService
    {
        public List<Driver> AllRegisteredDrivers { get; private set; }
        private List<Trip> AllTrips { get; set; }

        public void ProcessDriverCommand() //maybe this could just return the drivers every time for convenience, we'll see
        {

        }

        public void ProcessTripCommand()
        {

        }

        public IOrderedEnumerable<DriverTripSummary> GetDriverTripSummary()
        {
            return AllTrips.GroupBy(t => t.DriverName).Select(g => new DriverTripSummary()
            {
                DriverName = g.Key,
                TotalDistance = g.Sum(t => t.MilesDriven),
                TotalMinutes = Convert.ToInt32(g.Sum(t => t.EndTime.Subtract(t.StartTime).TotalMinutes)) // convert the sum rather than each item
            }).OrderByDescending(dts => dts.TotalDistance);
        }
    }
}
