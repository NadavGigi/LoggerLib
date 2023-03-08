using NUnit.Framework;
using System.Threading;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLib.Tests
{
    public class LoggerLibTests
    {
        private const string LogFilePath = "C:\\Users\\nadav\\OneDrive\\שולחן העבודה\\lab\\LoggerLib\\LoggerLib\\log.txt";

        [SetUp]
        public void SetUp()
        {
            // Delete log file before each test
            File.WriteAllText(LogFilePath, String.Empty);
        }
        [Test]
        public void PrintLogLine_MessageWrittenToFile()
        {
            // Arrange
            var fileWriter = new FileWriter(LogFilePath);
            var logger = new LoggerLib(fileWriter);

            // Act
            logger.PrintLogLine("Test message");
            Thread.Sleep(200);
            logger.Terminate();
            fileWriter.Dispose();

            // Assert
            var logContents = File.ReadAllText(LogFilePath).Trim();
            Assert.That(logContents, Is.EqualTo("Test message"));
        }

        [Test]
        public void Terminate_WaitForThreadToFinish()
        {
            // Arrange
            var fileWriter = new FileWriter(LogFilePath);
            var logger = new LoggerLib(fileWriter);

            // Act
            logger.PrintLogLine("Test message");
            Thread.Sleep(200);
            logger.Terminate();
            fileWriter.Dispose();
            // Assert
            Assert.That(logger.IsWorkerThreadAlive(), Is.False);
        }

        [Test]
        public void Filters_AppliedToMessages()
        {
            // Arrange
            var fileWriter = new FileWriter(LogFilePath);
            var logger = new LoggerLib(fileWriter, new List<Func<string, string>> { message => message.ToUpper() });

            // Act
            logger.PrintLogLine("Test message");
            Thread.Sleep(200);
            logger.Terminate();
            fileWriter.Dispose();
            // Assert
            var logContents = File.ReadAllText(LogFilePath).Trim();
            Assert.That(logContents, Is.EqualTo("TEST MESSAGE"));
        }

        [Test]
        public void MultipleLogLines_AllWrittenToFile()
        {
            // Arrange
            var fileWriter = new FileWriter(LogFilePath);
            var logger = new LoggerLib(fileWriter);
            String check = "";
            // Act
            for (int i = 0; i < 100; i++)
            {
                logger.PrintLogLine($"Number{i}");
                check += $"Number{i}";

                // Delay client printing
                Thread.Sleep(10);
            }
            Thread.Sleep(2000);
            logger.Terminate();
            fileWriter.Dispose();
            // Assert
            var logContents = File.ReadAllText(LogFilePath).Trim();
            Assert.That(logContents, Is.EqualTo(check));
        }
    }
}
