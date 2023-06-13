using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class ListViewEventItem
    {
        public Event Event { get; set; }
        public Color BackgroundColor => Event2Color(Event);

        public ListViewEventItem(Event e) 
        {
            Event = e;
        }

        public static Color Event2Color(Event e)
        {
            if (e.DDLDate < DateTime.Now || (e.DDLDate == DateTime.Now && e.DDLTime < DateTime.Now.TimeOfDay))
                return Color.FromRgba("#E6E6E6");   // light gray
            var urg = e.Urgency;
            if (urg == AllUrgency.Cake)
                return Color.FromRgba("#FDFCF0");   // light yellow
            else if (urg == AllUrgency.Routine)
                return Color.FromRgba("#D5D9FD");   // light blue
            else if (urg == AllUrgency.Vital)
                return Color.FromRgba("#FDD2FC");   // light pink
            else if (urg == AllUrgency.Urgent)
                return Color.FromRgba("#FDC4CF");   // light red
            else
                return Color.FromRgba("#FFFFFF");
        }
    }

    public class EventViewModel : INotifyPropertyChanged
    {
        private EventModel model;

        public ObservableCollection<ListViewEventItem> Events { get; set; } = new();

        private void RefreshEvents()
        {
            var modelEvents = model.GetEvents();
            Events.Clear();
            foreach (var e in modelEvents)
            {
                Events.Add(new ListViewEventItem(e));
            }
            this.storager.WriteIn(modelEvents);
        }

        private string newEventName = "";
        public string NewEventName
        {
            get => newEventName;
            set
            {
                newEventName = value;
                OnPropertyChanged(nameof(NewEventName));
            }
        }

        public ObservableCollection<string> AvailUrgency { get; } = new ObservableCollection<string>(Enum.GetNames(typeof(AllUrgency)));

        private string newEventUrgency = "";
        public string NewEventUrgency
        {
            get => newEventUrgency;
            set
            {
                newEventUrgency = value;
                OnPropertyChanged(nameof(NewEventUrgency));
            }
        }

        private TimeSpan newEventDDLTime = DateTime.Now.TimeOfDay;
        public TimeSpan NewEventDDLTime
        {
            get => newEventDDLTime;
            set
            {
                newEventDDLTime = value;
                OnPropertyChanged(nameof(NewEventDDLTime));
            }
        }

        private DateTime newEventDDLDate = DateTime.Now;
        public DateTime NewEventDDLDate
        {
            get => newEventDDLDate;
            set
            {
                newEventDDLDate = value;
                OnPropertyChanged(nameof(NewEventDDLDate));
            }
        }

        private string detail = "";
        public string Detail
        {
            get => detail;
            set
            {
                detail = value;
                OnPropertyChanged(nameof(Detail));
            }
        }

        public void ResetLabels()
        {
            this.NewEventName = "";
            this.NewEventUrgency = "";
            this.NewEventDDLDate = DateTime.Now;
            this.NewEventDDLTime = DateTime.Now.TimeOfDay;
            this.Detail = "";
        }

        public Command AddEventCommand { get; init; }
        private void CAddEvent()
        {
            if (this.NewEventName == "" || this.NewEventUrgency == "")
                return;
            var newEvent = new Event(this.NewEventName, Utils.GetUrgency(this.NewEventUrgency) ?? AllUrgency.Cake, NewEventDDLDate, NewEventDDLTime, this.Detail);
            model.AddEvent(newEvent);
            CRefresh();
        }

        public Command<ListViewEventItem> DeleteEventCommand { get; init; }
        private void CDeleteEvent(ListViewEventItem? e)
        {
            this.model.DeleteEvent(e?.Event);
            CRefresh();
        }

        public Command RefreshCommand { get; init; }
        private void CRefresh()
        {
            ResetLabels();
            RefreshEvents();
        }
        private ListViewEventItem? selectedEvent;
        public ListViewEventItem? SelectedEvent
        {
            get => selectedEvent;
            set
            {
                this.selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                if (selectedEvent is null) return;
                var tempEvent = selectedEvent.Event;
                this.NewEventName = tempEvent.Name;
                this.NewEventUrgency = tempEvent.Urgency.ToString();
                this.NewEventDDLDate = tempEvent.DDLDate;
                this.NewEventDDLTime = tempEvent.DDLTime;
                this.Detail = tempEvent.Detail;
            }
        }

        //public void OnTapEvent(object sender, ItemTappedEventArgs e)
        //{
        //    var item = e.Item as Event;
        //    if (item == null)
        //        return;
        //    if (item == SelectedEvent)
        //    {
        //        SelectedEvent = null;
        //    }
        //    else
        //    {
        //        SelectedEvent = item;
        //    }
            
        //}

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected DataStorager storager;

        public EventViewModel()
        {
            var appDataPath = FileSystem.AppDataDirectory;
            var filePath = Path.Combine(appDataPath, "data.json");
            this.storager = new(filePath);
            List<Event>? eventList = null;
            try
            {
                eventList = this.storager.ReadFrom();
            }
            catch { }
            model = new EventModel(eventList);
            this.AddEventCommand = new Command(CAddEvent);
            this.DeleteEventCommand = new Command<ListViewEventItem>(CDeleteEvent);
            this.RefreshCommand = new Command(CRefresh);
            RefreshEvents();
        }
    }
}

