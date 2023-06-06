using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{

    public class Event
    {
        public string Name { get; set; }
        public AllUrgency Urgency { get; set; }
        public string DDL { get; set; }
        public string Info => Urgency.ToString() + " | DDL: "+ DDL;
        public Event(string name, AllUrgency urgency, string ddL)
        {
            Name = name;
            Urgency = urgency;
            DDL = ddL;
        }
    }

    public class EventComparer : IComparer<Event>
    {
        public int Compare(Event? x, Event? y)
        {
            if (x is null)
                return y is null ? 0 : -1;
            if (y is null)
                return 1;
            return Utils.CompareUrgency(x.Urgency, y.Urgency);
        }
    }

    public class EventModel
    {
        private List<Event> events;

        public EventModel()
        {
            events = new List<Event>();
        }

        public void AddEvent(Event e)
        {
            events.Add(e);
        }

        public List<Event> GetEvents()
        {
            return events;
        }

        public void DeleteEvent(Event? e)
        {
            if (e is null) return;
            events.Remove(e);
        }
    }
}
