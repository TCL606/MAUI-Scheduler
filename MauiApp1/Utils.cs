using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{

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
            if (urg1 == AllUrgency.Urgent)
                return 1;
            if (urg1 == AllUrgency.Important && urg2 != AllUrgency.Urgent)
                return 1;
            if (urg1 == AllUrgency.Routine && urg2 != AllUrgency.Urgent && urg2 != AllUrgency.Important)
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

    public class SortedList<T>
    {
        public List<T> ObjectList { get; private set; }

        // T1 > T2 ? 1: T1 == T2 ? 0 : 1
        public Func<T, T, int> CompareFunc { get; }

        public SortedList(Func<T, T, int> func)
        {
            ObjectList = new List<T>();
            CompareFunc = func;
        }

        public void Add(T obj)
        {
            var index = ObjectList.FindIndex(x => CompareFunc(obj, x) >= 0);
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
