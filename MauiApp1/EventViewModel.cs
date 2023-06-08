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
    public class EventViewModel : INotifyPropertyChanged
    {
        private EventModel model;

        private ObservableCollection<Event> events = new();
        public ObservableCollection<Event> Events
        {
            get => events;
            set
            {
                events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private void RefreshEvents()
        {
            Events = new ObservableCollection<Event>(model.GetEvents());
            this.storager.WriteIn(model.GetEvents());
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

        public Command<Event> DeleteEventCommand { get; init; }
        private void CDeleteEvent(Event? e)
        {
            this.model.DeleteEvent(e);
            CRefresh();
        }

        public Command RefreshCommand { get; init; }
        private void CRefresh()
        {
            ResetLabels();
            RefreshEvents();
        }
        private Event? selectedEvent;
        public Event? SelectedEvent
        {
            get => selectedEvent;
            set
            {
                this.selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
                if (selectedEvent is null) return;
                this.NewEventName = selectedEvent.Name;
                this.NewEventUrgency = selectedEvent.Urgency.ToString();
                this.NewEventDDLDate = selectedEvent.DDLDate;
                this.NewEventDDLTime = selectedEvent.DDLTime;
                this.Detail = selectedEvent.Detail;
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
            this.DeleteEventCommand = new Command<Event>(CDeleteEvent);
            this.RefreshCommand = new Command(CRefresh);
            RefreshEvents();
        }
    }
}

