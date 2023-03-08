using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace LoggerLib
{
    public class LoggerLib : ILoggerLib
    {
        private readonly ConcurrentQueue<string> m_queue = new ConcurrentQueue<string>();
        private readonly IFileWriter m_fileWriter;
        private readonly List<Func<string, string>> m_filters;
        private bool stopFlag = false;
        private Thread m_workerThread;

        public LoggerLib(IFileWriter fileWriter, List<Func<string, string>> filters = null)
        {
            m_fileWriter = fileWriter;
            m_filters = filters ?? new List<Func<string, string>>();
            m_workerThread = new Thread(DoWork);
            m_workerThread.Start();
        }

        public void PrintLogLine(string message)
        {
            m_queue.Enqueue(message);
        }

        private void DoWork()
        {
            while (!stopFlag)
            {
                if (m_queue.TryDequeue(out string message))
                {
                    try
                    {
                        foreach (var filter in m_filters)
                        {
                            message = filter(message);
                        }
                        m_fileWriter.Write(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error with handling Filters or with file writer.err msg:\n"+ex.ToString());
                        // Stop processing further messages until the error is fixed
                        break;
                    }

                }
                // Delay backend printing (simulate IO delay - DO NOT CHANGE)
                Thread.Sleep(20);
            }
        }
        public bool IsWorkerThreadAlive()
        {
            return m_workerThread.IsAlive;
        }
        public void Terminate()
        {
            // Signal worker thread to exit the loop and terminate
            stopFlag = true;

            // Wait for worker thread to terminate
            m_workerThread.Join();
        }
    }
}
