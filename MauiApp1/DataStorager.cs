using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiApp1
{
    public class DataStorager
    {
        public string Filename { get; }

        public void WriteIn(List<Event> events)
        {
            var json = JsonConvert.SerializeObject(events);
            File.WriteAllText(Filename, json);
        }

        public List<Event>? ReadFrom()
        {
            var json = File.ReadAllText(Filename);
            return JsonConvert.DeserializeObject<List<Event>>(json);

        }

        public DataStorager(string filename)
        {
            Filename = filename;
            if (!Filename.EndsWith(".json"))
                Filename += ".json";
        }

    }
}
