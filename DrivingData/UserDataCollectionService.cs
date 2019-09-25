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

        //TODO case if driver already exists
        public void RegisterDriver(string driverName)
        {
            RegisterDriver(new Driver(driverName));
        }
        public void RegisterDriver(Driver d)
        {
            AllRegisteredDrivers.Add(d);
        }

        public void RegisterTrip(Driver d, DateTime startTime, DateTime endTime, decimal milesDriven)
        {
            RegisterTrip(new Trip(d.Name, startTime, endTime, milesDriven));
        }

        public void RegisterTrip(Trip t)
        {
            AllTrips.Add(t);
        }

        public IOrderedEnumerable<DriverTripSummary> GetDriverTripSummaries()
        {
            var nonzeroSummaries = AllTrips.GroupBy(t => t.DriverName).Select(g => new DriverTripSummary()
            {
                DriverName = g.Key,
                TotalDistance = g.Sum(t => t.MilesDriven),
                TotalMinutes = Convert.ToInt32(g.Sum(t => t.EndTime.Subtract(t.StartTime).TotalMinutes)) // convert the sum rather than each item
                //Note that the elapsed time is calculated here and in Trip.cs. They should probably get moved/abstracted but the 
                //function is essentially just .Subtract and the liklihood of needing to change it is slim. I'm going to rule that the
                //performance gain here is pragmatically worth the loss of DRY and just leave this comment if it ever matters.
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
