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
        public void CAddEvent()
        {
            if (this.NewEventName == "" || this.newEventUrgency == "")
                return;
            var newEvent = new Event(this.newEventName, Utils.GetUrgency(this.newEventUrgency) ?? AllUrgency.Cake, this.NewEventDDLDate.ToShortDateString() + " " + this.NewEventDDLTime.ToString(@"hh\:mm"), this.Detail);
            model.AddEvent(newEvent);
            RefreshEvents();
            ResetWhenPushEvent();
        }

        public Command<Event> DeleteEventCommand { get; init; }
        public void CDeleteEvent(Event? e)
        {
            this.model.DeleteEvent(e);
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

        public EventViewModel()
        {
            model = new EventModel();
            Events = new ObservableCollection<Event>(model.GetEvents());
            this.AddEventCommand = new Command(CAddEvent);
            this.DeleteEventCommand = new Command<Event>(CDeleteEvent);
        }
    }
}

