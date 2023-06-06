using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MauiApp1
{

    public class Event: IComparable<Event>
    {
        public string Name { get; set; }
        public AllUrgency Urgency { get; set; }
        public string DDL { get; set; }
        public string Info => Urgency.ToString() + " | DDL: " + DDL;
        public string Detail { get; set; }
        public string DetailInfo => GetDetailInfo();

        private string GetDetailInfo()
        {
            return $"Event: {Name} \n" +
                $"Urgency: {Urgency.ToString()} \n" +
                $"DDL: {DDL} \n" +
                $"Detail: {Detail}";
        }

        public int CompareTo(Event? e)
        {
            return Utils.CompareUrgency(this.Urgency, e?.Urgency);
        }

        public Event(string name, AllUrgency urgency, string ddL, string detail = "")
        {
            Name = name;
            Urgency = urgency;
            DDL = ddL;
            Detail = detail;
        }
    }

    public class EventModel
    {
        private SortedList<Event> events;

        public EventModel(List<Event>? eventList = null)
        {
            events = new SortedList<Event>((Event x, Event y) => Utils.CompareUrgency(x.Urgency, y.Urgency), eventList);
        }

        public void AddEvent(Event e)
        {
            events.Add(e);
        }

        public List<Event> GetEvents()
        {
            return events.ObjectList;
        }

        public void DeleteEvent(Event? e)
        {
            if (e is null) return;
            events.Remove(e);
        }
    }
}
