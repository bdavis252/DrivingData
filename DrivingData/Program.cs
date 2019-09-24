using System;

namespace DrivingData
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will work for now but really ought to have DI/IoC.
            // As both of the bottom ones depend on the same instance of the top, for now, meh.
            var udcs = new UserDataCollectionService();
            var rs = new ReportService(udcs);
            var ts = new TextFileService(udcs);

            Console.WriteLine("Hello World!");

            //if args < 1 abort
            //else use args to get filename

            //feed file name to text service.
            
            Console.ReadLine(); //keep window open until enter pressed to see output
        }
    }
}
