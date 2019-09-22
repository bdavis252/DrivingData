using System;

namespace DrivingData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //if args < 1 abort
            //else use args to get filename

            //feed file name to text service. I think that service should depend on something that will process the user data.
            //that data should probably get persisted somewhere till this main method calls the report service. 
            //Probably user service would be fine.
            //then the report service would say hey user service, give me the most current list of stuff.
            
            Console.ReadLine(); //keep window open until enter pressed to see output
        }
    }
}
