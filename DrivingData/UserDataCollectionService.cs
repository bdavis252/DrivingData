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
        public UserDataCollectionService()
        {
            AllRegisteredDrivers = new List<Driver>();
            AllTrips = new List<Trip>();
        }

        //TODO case if driver does not exist
        public void RegisterDriver(string driverName)
        {
            AllRegisteredDrivers.Add(new Driver(driverName));
        }
        public void RegisterDriver(Driver d)
        {
            AllRegisteredDrivers.Add(d);
        }

        public void RegisterTrip(Driver d, DateTime startTime, DateTime endTime, decimal milesDriven)
        {
            AllTrips.Add(new Trip()
            {
                DriverName = d.Name,
                StartTime = startTime,
                EndTime = endTime,
                MilesDriven = milesDriven
            });
        }

        public IOrderedEnumerable<DriverTripSummary> GetDriverTripSummaries()
        {
            //TODO discard < 5 and > 100

            var nonzeroSummaries = AllTrips.GroupBy(t => t.DriverName).Select(g => new DriverTripSummary()
            {
                DriverName = g.Key,
                TotalDistance = g.Sum(t => t.MilesDriven),
                TotalMinutes = Convert.ToInt32(g.Sum(t => t.EndTime.Subtract(t.StartTime).TotalMinutes)) // convert the sum rather than each item
            });

            //This is a little strange because strings aren't unique; would be much better to do this with Ids. See comment in Driver class
            var usersWithNoTrips = AllRegisteredDrivers.Select(x => x.Name).Except(nonzeroSummaries.Select(y => y.DriverName));
            var summariesForUsersWithNoTrips = usersWithNoTrips.Select(u => new DriverTripSummary()
            {
                DriverName = u // other fields will default to 0
            });

            return nonzeroSummaries
                .Concat(summariesForUsersWithNoTrips)
                .OrderByDescending(dts => dts.TotalDistance)
                .ThenBy(dts => dts.DriverName);
        }
    }
}
