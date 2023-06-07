using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MauiApp1
{

    public class Event : IComparable<Event>
    {
        public string Name { get; set; }
        public AllUrgency Urgency { get; set; }
        public DateTime DDLDate { get; set; }
        public TimeSpan DDLTime { get; set; }
        public string Info => Urgency.ToString() + " | DDL: " + DDLDate.ToString("MM/dd") + " " + DDLTime.ToString(@"hh\:mm");
        public string Detail { get; set; }
        public string DetailInfo => GetDetailInfo();

        private string GetDetailInfo()
        {
            return $"Event: {Name} \n" +
                $"Urgency: {Urgency.ToString()} \n" +
                $"DDL: {DDLDate.ToShortDateString() + " " + DDLTime.ToString(@"hh\:mm")} \n" +
                $"Detail: {Detail}";
        }

        public int CompareTo(Event? e)
        {
            var val = Utils.CompareUrgency(this.Urgency, e?.Urgency);
            if (val != 0)
                return val;

            // TODO
            return 0;
        }

        public Event(string name, AllUrgency urgency, DateTime ddLDate, TimeSpan ddLTime, string detail = "")
        {
            Name = name;
            Urgency = urgency;
            DDLDate = ddLDate;
            DDLTime = ddLTime;
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
