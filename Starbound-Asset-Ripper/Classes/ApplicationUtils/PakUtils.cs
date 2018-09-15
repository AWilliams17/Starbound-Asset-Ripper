using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace ApplicationUtils
{
    public static class PakUtils
    {
        public static void UnpackPakFile(string SteamPath, string PakFilePath, string OutputPath)
        {
           
        }

        public static void UnpackMultiplePaks(string SteamPath, string OutputPath, ListBox PakListBox, Dictionary<string, string> PakDictionary)
        {
            
        }
    }
    
    public class UnpackPakException : Exception
    {
        public UnpackPakException() { }
        public UnpackPakException(string message) : base(message) { }
        public UnpackPakException(string message, Exception inner) : base(message, inner) { }
        protected UnpackPakException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
