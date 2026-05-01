using System;
using System.Xml.Linq;
class TaskItem
{
    public int number;
    private string _priority;
    public string Priority
    {
        get => _priority;
        set
        {
            if (!(value=="высокий"|| value == "средний"|| value == "низкий"))
            {
                throw new FormatException("неверный формат ввода");
            }
            _priority = value;
        }
    }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentException( "имя не может быть пустым"); }
            _name = value;
        }
    }
    public string zadacha => $"{number}) {_name}, {_priority} приоритет";
}


class Program
{
    static void Main()
    {
        List <TaskItem> Biglist = new List <TaskItem> ();
        while (true)
        {
            Console.WriteLine("Введите задачу");
            TaskItem item = new TaskItem();
            item.number = Biglist.Count + 1;
            bool work = false;
            while (!work)
            {
                try 
                {
                    item.Name = Console.ReadLine();
                    work = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("введите приоритет");
            work = false;
            while (!work)
            {
                try
                {
                    item.Priority = Console.ReadLine();
                    work = true;
                }
                catch ( FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Biglist.Add(item);
            foreach (TaskItem i in Biglist)
            {
                Console.WriteLine(i.zadacha);
            }

        }
        
    }
}