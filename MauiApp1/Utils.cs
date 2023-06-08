using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{
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
        Vital = 3,
        Routine = 2,
        Cake = 1,
    }

    public static class Utils
    {
        public static int CompareUrgency(AllUrgency? urg1, AllUrgency? urg2)
        {
            if (urg1 is null && urg2 is null)
                return 0;
            if (urg1 is null) 
                return -1;
            if (urg2 is null) 
                return 1;
            if (urg1 == urg2)
                return 0;
            if (urg1 == AllUrgency.Urgent)
                return 1;
            if (urg1 == AllUrgency.Vital && urg2 != AllUrgency.Urgent)
                return 1;
            if (urg1 == AllUrgency.Routine && urg2 != AllUrgency.Urgent && urg2 != AllUrgency.Vital)
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

    public class SortedList<T> where T : IComparable<T>
    {
        public List<T> ObjectList { get; private set; }

        public int Count => ObjectList.Count;

        public T this[int i]
        {
            get => ObjectList[i];
        }

        public SortedList(List<T>? objectList = null)
        {
            ObjectList = objectList is null ? new List<T>() : objectList.OrderByDescending(i => i).ToList();
        }

        public void Add(T obj)
        {
            var index = ObjectList.FindIndex(x => obj.CompareTo(x) >= 0);
            if (index == -1)
                ObjectList.Add(obj);
            else
                ObjectList.Insert(index, obj);
        }

        public bool Remove(T obj)
        {
            return ObjectList.Remove(obj);
        }
    }
}
