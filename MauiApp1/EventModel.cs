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
        public string Info => Urgency.ToString() + " | DDL: " + DDLDate.ToShortDateString() + " " + DDLTime.ToString(@"hh\:mm");
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
            if (e is null) 
                return 1;
            var val = Utils.CompareUrgency(this.Urgency, e.Urgency);
            if (val != 0)
                return val;
            
            if (this.DDLDate < e.DDLDate)
                return 1;
            else if (this.DDLDate > e.DDLDate)
                return -1;
            
            if (this.DDLTime < e.DDLTime) 
                return 1;
            else if (this.DDLTime > e.DDLTime) 
                return -1;

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
            events = new SortedList<Event>(eventList);
        }

        public void AddEvent(Event e)
        {
            for (int i = 0; i < events.Count; i++)
            {
                if (e.Name == events[i].Name)
                {
                    events.Remove(events[i]);
                    break;
                }
            }
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
