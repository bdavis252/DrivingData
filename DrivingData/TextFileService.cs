using System;
using System.Collections.Generic;
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
            //use filename and load file
            //Path p
        }

        /// <summary>
        /// Eats a command and if good, persists it
        /// </summary>
        /// <param name="command">The command (e.g. Driver Dan)</param>
        public void ProcessDriverCommand(string command)
        {
            
        }

        /// <summary>
        /// Eats a command and if good, persists it
        /// </summary>
        /// <param name="command">The command (e.g. Trip Dan 07:15 07:45 17.3)</param>
        public void ProcessTripCommand(string command)
        {

        }
    }
}
