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

class TaskManager
{
    static public void SaveAll(List<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks);
        File.WriteAllText("tasks.json", json);
    }
    static public List<TaskItem> LoadTasks()
    {
        try
        {
            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке задач");
        }
        return new List<TaskItem>();
    }

    static public void ShowAll(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
        }
        else
        {
            foreach (TaskItem i in tasks)
            {
                Console.WriteLine(i.zadacha);
            }
        }

    }

    static public void ItemAdd(TaskItem item1, List<TaskItem> Biglist)
    {
        item1.number = Biglist.Count + 1;
        bool work = false;
        while (!work)
        {
            try
            {
                item1.Name = Console.ReadLine();
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
                item1.Priority = Console.ReadLine();
                work = true;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        Biglist.Add(item1);
        string json = JsonSerializer.Serialize(Biglist);
        File.WriteAllText("tasks.json", json);
    }

    static public void DeleteTask(List<TaskItem> tasks)
    {
        ShowAll(tasks);
        Console.WriteLine("введите номер задачи, которую нужно удалить");
        int input = Convert.ToInt32(Console.ReadLine());
        tasks.RemoveAt(input - 1);
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].number = i + 1;
        }
        SaveAll(tasks);
    }

    static public void DeleteAll(List<TaskItem> tasks)
    {
        Console.Write("вы уверены, что хотите удалить все задачи?\n для подтверждения напишите yes на клавиатуре");
        string input = Console.ReadLine();
        if (input == "yes")
        {
            tasks.Clear();
            SaveAll(tasks);
            Console.WriteLine("задачи удалены");
        }
        else { Console.WriteLine("операция отменена"); }

    }
}

class Program
{
  
    static void Main()
    {
        List<TaskItem> Biglist = TaskManager.LoadTasks();
        bool process = true;
        while (process)
        {
            Console.WriteLine("что ты хочешь от меня?");
            Console.WriteLine("1/2/3/4/5");
            Console.WriteLine("\n 1 - добавление новой задачи \n 2 - вывести все задачи \n 3 - выйти \n 4 - удалить задачу \n 5 - удалить все задачи\n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Введите задачу");
                    TaskItem item = new TaskItem();
                    TaskManager.ItemAdd(item, Biglist);
                    break;
                case "2":
                    TaskManager.ShowAll(Biglist);
                    break;
                case "3":
                    process = false;
                    break;
                case "4":
                    TaskManager.DeleteTask(Biglist);
                    break;
                case "5":
                    TaskManager.DeleteAll(Biglist);
                    break;
                default:
                    Console.WriteLine("перечитай инструкцию");
                    break;

            }           

        }
        
    }
}