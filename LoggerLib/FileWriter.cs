using System;
using System.IO;

public class FileWriter : IFileWriter
{
    private readonly TextWriter m_writer;

    public FileWriter(string filePath = null)
    {
        if (filePath != null)
        {
            m_writer = new StreamWriter(filePath);
        }
        else
        {
            m_writer = Console.Out;
        }
        
    }

    public void Write(string line)
    {
        m_writer.Write(line);
    }

    public void Dispose()
    {
        m_writer.Dispose();
    }
}
