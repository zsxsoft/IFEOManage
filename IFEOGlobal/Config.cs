using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace IFEOGlobal
{
    [DataContract]
    public class ConfigObject
    {
        [DataMember]
        public int Timeout = 5;
        [DataMember]
        public bool Log = true;
    }
    public class Config
    {
        public static ConfigObject Data = new ConfigObject();
        public static bool Load()
        {
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(ConfigObject));
            FileStream Fs = File.Open(System.AppDomain.CurrentDomain.BaseDirectory + "\\Config.json", FileMode.Open);
            Data = (ConfigObject)Serializer.ReadObject(Fs);
            return true;
        } 

        public static bool Save()
        {
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(ConfigObject));
            FileStream Fs = File.Open(System.AppDomain.CurrentDomain.BaseDirectory + "\\Config.json", FileMode.Create, FileAccess.Write);
            Serializer.WriteObject(Fs, Data);
            Fs.Close();
            return true;
        }
    }
}
