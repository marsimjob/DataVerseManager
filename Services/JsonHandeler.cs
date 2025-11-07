using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataVerseManager.Services
{
    public class JsonHandeler
    {
        // This is a generic Json Reading Method, it saves parameter to path
        public static void SaveJson<T>(T readObject, string path)
        {
            try
            {
                string json = JsonSerializer.Serialize(readObject,
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(path, json);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
        }
       public static T LoadJson<T>(string path)
        { 
            try
            {
                string json = File.ReadAllText(path);
                T loadObject = JsonSerializer.Deserialize<T>(json);

                return loadObject;
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                return default(T);
            }
        }
    }
}


