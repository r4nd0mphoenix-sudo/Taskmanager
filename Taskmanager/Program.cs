using System;
using System.IO;
using System.Text.Json;
class TaskItem
{
    public int number { get; set; }
    public string Priority { get; set; }
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
    public string done { get; set; }
    public string zadacha => $"{number}) {_name}, {Priority} приоритет, {done}";
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
        Console.WriteLine("введите приоритет\n");
        Console.WriteLine(" 1 - высокий \n 2 - средний \n 3 - низкий \n");
        string input = Console.ReadLine();
        work = false;
        while (!work)
        {
            switch (input)
            {
                case "1":
                    item1.Priority = "высокий";
                    work = true;
                    break;
                case "2":
                    item1.Priority = "средний";
                    work = true;
                    break;
                case "3":
                    item1.Priority = "низкий";
                    work = true;
                    break;
                default: 
                    Console.WriteLine("неправильный формат ввода, попробуйте еще раз");
                    input = Console.ReadLine();
                    break;
            }
        }
        item1.done = " [-]";
        SaveAll(Biglist);
    }
    static public void DeleteTask(List<TaskItem> tasks)
    {
        ShowAll(tasks);
        Console.WriteLine("введите номер задачи, которую нужно удалить");
        bool work = true;
        while (work)
        {
            int input = Convert.ToInt32(Console.ReadLine());
            if (input >= 1 && input <= tasks.Count)
            {
                tasks.RemoveAt(input - 1);
                for (int i = 0; i < tasks.Count; i++)
                {
                    tasks[i].number = i + 1;
                }
                SaveAll(tasks);
                work = false;
                Console.WriteLine("Задача удалена\n");
            }
            else Console.WriteLine("введенный номер задачи не существует, попробуйте еще раз");
        }
    }
    static public void redact (List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер задачи, которую нужно отредактировать \n");
        bool work = false;
        while (!work)
        {
            int input = Convert.ToInt32(Console.ReadLine());
            if (input >= 1 && input <= tasks.Count)
            {
                Console.WriteLine("введите задачу\n");
                tasks[input - 1].number = input;
                TaskManager.ItemAdd(tasks[input-1], tasks);
                work = true;
            }
            else
            {
                Console.WriteLine("введенный номер не существует, попробуйте еще раз \n");
            }

        }
    }
    static public void itdone(List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер выполненной задачи\n");
        bool work = false;
        while (!work)
        {
            int input = Convert.ToInt32(Console.ReadLine());
            if (input >= 1 && input <= tasks.Count)
            {
                tasks[input - 1].done = "[++выполнена++]";
                work = true;
            }
            else
            {
                Console.WriteLine("введенный номер не существует, попробуйте еще раз \n");
            }

        }
    }
    static public void DeleteAll(List<TaskItem> tasks)
    {
        Console.Write("вы уверены, что хотите удалить все задачи?\n для подтверждения напишите да на клавиатуре\n");
        string input = Console.ReadLine();
        if (input == "да")
        {
            tasks.Clear();
            SaveAll(tasks);
            Console.WriteLine("задачи удалены\n");
        }
        else { Console.WriteLine("операция отменена\n"); }

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
            Console.WriteLine("\nчто ты хочешь от меня?\n");
            Console.WriteLine("1/2/3/4/5/6/7");
            Console.WriteLine("\n 1 - добавление новой задачи \n 2 - вывести все задачи \n 3 - выйти \n 4 - удалить задачу \n 5 - удалить все задачи\n 6 - редактировать задачу\n 7 - отметить как выполненную\n");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Введите задачу");
                    TaskItem item = new TaskItem();
                    item.number = Biglist.Count + 1;
                    TaskManager.ItemAdd(item, Biglist);
                    Console.WriteLine(item.zadacha);
                    Biglist.Add(item);
                    Console.WriteLine("задача успешно сохранена\n");
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
                case "6":
                    TaskManager.redact(Biglist);
                    break;
                case "7":
                    TaskManager.itdone(Biglist);
                    TaskManager.SaveAll(Biglist);
                    break;
                default:
                    Console.WriteLine("перечитай инструкцию");
                    break;

            }           

        }
        
    }
}