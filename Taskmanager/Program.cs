using System;
using System.IO;
using System.Text.Json;
class TaskItem
{
    public int number { get; set; }
    private string _priority;
    public string Priority
    {
        get => _priority;
        set
        {
            if (!(value=="высокий"|| value == "средний"|| value == "низкий")|| string.IsNullOrWhiteSpace(value))
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
    static List<TaskItem> LoadTasks()
    {
        try
        {
            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке задач");
        }
        return new List<TaskItem>();
    }
    static void Main()
    {
        List<TaskItem> Biglist = LoadTasks();
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
            string json = JsonSerializer.Serialize(Biglist);
            File.WriteAllText("tasks.json", json);
            foreach (TaskItem i in Biglist)
            {
                Console.WriteLine(i.zadacha);
            }

        }
        
    }
}