using DrivingData.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace DrivingData
{
    // So this isn't a real web API but if it were these would be service endpoints
    public class TextFileService
    {
        private UserDataCollectionService udcs;

        public TextFileService(UserDataCollectionService udcs)
        {
            this.udcs = udcs;
        }

        /// <summary>
        /// Reads text file. Processes each line in a loop.
        /// </summary>
        /// <param name="filename">Param from command line fed in.</param>
        public void ReadAndProcessTextFile(string filename)
        {
            //TODO fail gracefully?
            string[] lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                if (line.StartsWith("Driver "))
                {
                    ProcessDriverCommand(line);
                }
                else if(line.StartsWith("Trip "))
                {
                    ProcessTripCommand(line);
                }
                //TODO else fail gracefully?
            }
        }

        /// <summary>
        /// Parses driver command. Persists the driver.
        /// </summary>
        /// <param name="command">The command (e.g. Driver Dan)</param>
        public void ProcessDriverCommand(string command)
        {
            udcs.RegisterDriver(command.Replace("Driver ", string.Empty));
        }

        /// <summary>
        /// Attempts to parse a trip command, and if the data fits the model, persists the trip.
        /// </summary>
        /// <param name="command">The command (e.g. Trip Dan 07:15 07:45 17.3)</param>
        public void ProcessTripCommand(string command)
        {
            command = command.Replace("Trip ", string.Empty);
            try
            {
                var timeAndDistanceStrings = command.Split(' ');
                if (timeAndDistanceStrings.Length != 4) throw new Exception();

                var driver = new Driver(timeAndDistanceStrings[0]);
                var startTime = DateTime.ParseExact(timeAndDistanceStrings[1], "HH:mm", CultureInfo.InvariantCulture);
                var endTime = DateTime.ParseExact(timeAndDistanceStrings[2], "HH:mm", CultureInfo.InvariantCulture);
                var distance = decimal.Parse(timeAndDistanceStrings[3]);
                
                // Discard any trips that average a speed of less than 5 mph or greater than 100 mph.
                //TODO discard < 5 and > 100)

                udcs.RegisterTrip(driver, startTime, endTime, distance);
            }
            catch (Exception e)
            {
                throw new InvalidDataException("Data input was not formatted as valid times or distance.");
            }
        }
    }
}
