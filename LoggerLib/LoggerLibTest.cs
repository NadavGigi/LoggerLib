using System.Threading;
using System;
using System.Collections.Generic;
namespace LoggerLib
{
    class LoggerLibTest
    {
        static void Main(string[] args)
        {
            Func<string, string> addDateTimeFilter = message =>
            {
                return DateTime.Now.ToString() + ": " + message;
            };
            IFileWriter fileWriter = new FileWriter();
            ILoggerLib logger = new LoggerLib(fileWriter, new List<Func<string, string>> { addDateTimeFilter });
            for (int i = 0; i < 100; i++)
            {

                logger.PrintLogLine($"Number {i} \n");

                // Delay client printing
                Thread.Sleep(10);
            }
            Thread.Sleep(2000);
            //i couldnt fix the problem of writing only half of the messeges
            logger.Terminate();
        }
    }
}
