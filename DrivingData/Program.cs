using System;

namespace DrivingData
{
    class Program
    {
        static void Main(string[] args)
        {
            // This will work for now but really ought to have DI/IoC.
            var bs = new BusinessService();
            var udcs = new UserDataCollectionService(bs);
            var rs = new ReportService(bs, udcs);
            var tfs = new TextFileService(udcs);

            //if args < 1 abort
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a filename as an argument to this program.");
            }
            else
            {
                foreach (var filename in args)
                {
                    tfs.ReadAndProcessTextFile(filename);
                }
                Console.Write(rs.GenerateReport());
            }
            
            Console.ReadKey(); //keep window open until enter pressed to see output
        }
    }
}
