using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private EventModel model;

        public ObservableCollection<Event> Events { get; private set; }

        private void RefreshEvents()
        {
            Events.Clear();
            foreach (var e in model.GetEvents())
            {
                Events.Add(e);
            }
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

        public void ResetWhenPushEvent()
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
            if (this.NewEventName == "" || this.newEventUrgency == "")
                return;
            var newEvent = new Event(this.newEventName, Utils.GetUrgency(this.newEventUrgency) ?? AllUrgency.Cake, NewEventDDLDate, NewEventDDLTime, this.Detail);
            model.AddEvent(newEvent);
            RefreshEvents();
            ResetWhenPushEvent();
        }

        public Command<Event> DeleteEventCommand { get; init; }
        private void CDeleteEvent(Event? e)
        {
            this.model.DeleteEvent(e);
            RefreshEvents();
        }

        public Command RefreshCommand { get; init; }
        private void CRefresh()
        {
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
            Events = new ObservableCollection<Event>(model.GetEvents());
            this.AddEventCommand = new Command(CAddEvent);
            this.DeleteEventCommand = new Command<Event>(CDeleteEvent);
            this.RefreshCommand = new Command(CRefresh);
            RefreshEvents();
        }
    }
}

