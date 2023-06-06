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

        public void AddEvent(Event e)
        {
            model.AddEvent(e);
            RefreshEvents();
        }

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

        public void ResetWhenPushEvent()
        {
            this.NewEventName = "";
            this.NewEventUrgency = "";
            this.NewEventDDLDate = DateTime.Now;
            this.NewEventDDLTime = DateTime.Now.TimeOfDay;
        }

        public Command AddEventCommand { get; init; }
        public void CAddEvent()
        {
            if (this.NewEventName == "" || this.newEventUrgency == "")
                return;
            var newEvent = new Event(this.newEventName, Utils.GetUrgency(this.newEventUrgency) ?? AllUrgency.Cake, this.NewEventDDLDate.ToShortDateString() + " " + this.NewEventDDLTime.ToString(@"hh\:mm"));
            this.AddEvent(newEvent);
            ResetWhenPushEvent();
        }

        public Command<Event> DeleteEventCommand { get; init; }
        public void CDeleteEvent(Event? e)
        {
            this.model.DeleteEvent(e);
        }


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

    public class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => exeFunc();

        private Action exeFunc;

        public Command(Action exe)
        {
            this.exeFunc = exe;
        }
    }

    public class Command<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged { add { } remove { } }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => exeFunc((T?)parameter);

        private Action<T?> exeFunc;

        public Command(Action<T?> exe)
        {
            this.exeFunc = exe;
        }
    }

    public enum AllUrgency
    {
        Urgent = 4,
        Important = 3,
        Routine = 2,
        Cake = 1,
    }

    public static class Utils
    {
        public static int CompareUrgency(AllUrgency urg1, AllUrgency urg2)
        {
            if (urg1 == urg2)
                return 0;
            if (urg1 == AllUrgency.Urgent && urg2 != AllUrgency.Urgent)
                return 1;
            if (urg1 == AllUrgency.Important && urg2 != AllUrgency.Important)
                return 1;
            if (urg1 == AllUrgency.Routine && urg2 != AllUrgency.Routine)
                return 1;
            return -1;
        }

        public static AllUrgency? GetUrgency(string name)
        {
            try
            {
                return (AllUrgency)Enum.Parse(typeof(AllUrgency), name);
            }
            catch
            {
                return null;
            }
        }
    }
}


