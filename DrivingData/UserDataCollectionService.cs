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
        private BusinessService bs;

        public List<Driver> AllRegisteredDrivers { get; private set; }
        private List<Trip> AllTrips { get; set; }

        public UserDataCollectionService(BusinessService bs)
        {
            this.bs = bs;
            AllRegisteredDrivers = new List<Driver>();
            AllTrips = new List<Trip>();
        }

        /// <summary>
        /// Run any checks to see if driver can be registered, then registers them if needed.
        /// </summary>
        /// <param name="driverName"></param>
        public void CheckDriverThenRegister(string driverName)
        {
            CheckDriverThenRegister(new Driver(driverName));
        }
        public void CheckDriverThenRegister(Driver d)
        {
            //TODO handle case if driver already exists - YAGNI for now

            //If all the checks check out,
            ActuallyRegisterDriver(d);
        }

        /// <summary>
        /// This actually persists the driver in the database. 
        /// </summary>
        /// <param name="d">A vetted, bona fide new driver</param>
        private void ActuallyRegisterDriver(Driver d)
        {
            AllRegisteredDrivers.Add(d);
        }

        /// <summary>
        /// Run any checks to see if trip can be registered, then registers it if needed.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="milesDriven"></param>
        public void CheckTripThenRegister(Driver d, DateTime startTime, DateTime endTime, decimal milesDriven)
        {
            CheckTripThenRegister(new Trip(d.Name, startTime, endTime, milesDriven));
        }

        public void CheckTripThenRegister(Trip t)
        {
            // Discard any trips that average a speed of less than 5 mph or greater than 100 mph.
            var mph = bs.GetRoundedMph(t.MilesDriven, bs.GetMinutesElapsed(t));
            if (mph < 5 || mph > 100)
            {
                //For now, just discard. Later we may want to log it or something.
            }
            else
            {
                ActuallyRegisterTrip(t);
            }
        }


        private void ActuallyRegisterTrip(Trip t)
        {
            AllTrips.Add(t);
        }

        public IOrderedEnumerable<DriverTripSummary> GetDriverTripSummaries()
        {
            var nonzeroSummaries = AllTrips.GroupBy(t => t.DriverName).Select(g => new DriverTripSummary()
            {
                DriverName = g.Key,
                TotalDistance = g.Sum(t => t.MilesDriven),
                TotalMinutes = Convert.ToInt32(g.Sum(t => bs.GetMinutesElapsed(t))) // convert the sum rather than each item
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
