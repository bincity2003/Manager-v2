using System;
using System.IO;

namespace Manager.Library
{
    public static class LogWriter
    {
        public static void Write(string path, string data, bool includeTime)
        {
            if (includeTime)
            {
                data = $"{DateTime.Now}: " + data;
            }
            File.AppendAllText(path, data + "\n");
        }
    }
}
