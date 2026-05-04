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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Список задач:");
            Console.ResetColor();
            foreach (TaskItem i in tasks)
            {
                Console.WriteLine(i.zadacha);
            }
        }

    }
    static public void ItemAdd(List<TaskItem> Biglist)
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
        Console.WriteLine("введите приоритет\n");
        Console.WriteLine(" 1 - высокий \n 2 - средний \n 3 - низкий \n");
        string input = Console.ReadLine();
        work = false;
        while (!work)
        {
            switch (input)
            {
                case "1":
                    item.Priority = "высокий";
                    work = true;
                    break;
                case "2":
                    item.Priority = "средний";
                    work = true;
                    break;
                case "3":
                    item.Priority = "низкий";
                    work = true;
                    break;
                default:
                    Console.WriteLine("неправильный формат ввода, попробуйте еще раз");
                    input = Console.ReadLine();
                    break;
            }
        }
        item.done = "[ -]";
        Biglist.Add(item);
        SaveAll(Biglist);
        Console.WriteLine(item.zadacha);
        Console.WriteLine("задача успешно сохранена\n");
    }
    static public void DeleteTask(List<TaskItem> tasks)
    {
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
    static public void redact(List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер задачи, которую нужно отредактировать \n");
        bool work1 = false;
        while (!work1)
        {
            int input = Convert.ToInt32(Console.ReadLine());
            if (input >= 1 && input <= tasks.Count)
            {
                Console.WriteLine("введите задачу\n");
                tasks[input - 1].number = input;
                bool work = false;
                while (!work)
                {
                    try
                    {
                        tasks[input - 1].Name = Console.ReadLine();
                        work = true;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine("введите приоритет\n");
                Console.WriteLine(" 1 - высокий \n 2 - средний \n 3 - низкий \n");
                string inputp = Console.ReadLine();
                work = false;
                while (!work)
                {
                    switch (inputp)
                    {
                        case "1":
                            tasks[input - 1].Priority = "высокий";
                            work = true;
                            break;
                        case "2":
                            tasks[input - 1].Priority = "средний";
                            work = true;
                            break;
                        case "3":
                            tasks[input - 1].Priority = "низкий";
                            work = true;
                            break;
                        default:
                            Console.WriteLine("неправильный формат ввода, попробуйте еще раз");
                            inputp = Console.ReadLine();
                            break;
                    }
                }
                work1 = true;
                SaveAll(tasks);
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
                Console.WriteLine($"задача {input} выполнена!");
                TaskManager.SaveAll(tasks);
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
    static public void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("======МЕНЮ======\n");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Работа с задачами");
        Console.ResetColor();
        Console.WriteLine("\n 1 - создать задачу \n 2 - отметить выполненой \n 3 - удалить \n 4 - редактировать \n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Система");
        Console.ResetColor();
        Console.WriteLine("\n 5 - удалить все задачи\n 6 - выход\n 7 - сортировка \n");
    }
    static public void Sort(List<TaskItem> tasks)
    {
        Console.WriteLine("какие задачи вас интересуют?");
        Console.WriteLine(" 1 - высокий приоритет \n 2 - средний \n 3 - низкий \n 4 - выполненые \n 5 - невыполненые ");
        string input = Console.ReadLine();
        bool work = false;
        while (!work)
        {
            switch (input)
            {
                case "1":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.Priority == "высокий")
                        {
                            Console.WriteLine(i.zadacha);
                        }
                    }
                    work = true;
                    break;
                case "2":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.Priority == "средний")
                        {
                            Console.WriteLine(i.zadacha);
                        }
                    }
                    work = true;
                    break;
                case "3":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.Priority == "низкий")
                        {
                            Console.WriteLine(i.zadacha);
                        }
                    }
                    work = true;
                    break;
                case "4":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.done == "[++выполнена++]")
                        {
                            Console.WriteLine(i.zadacha);
                        }
                    }
                    work = true;
                    break;
                case "5":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.done == " [-]")
                        {
                            Console.WriteLine(i.zadacha);
                        }
                    }
                    work = true;
                    break;
                default: Console.WriteLine("неправильный формат ввода, попробуйте еще раз"); break;

            }

        }
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
                if (Biglist.Count > 0)
                {
                    Console.WriteLine("\n");
                    TaskManager.ShowAll(Biglist);
                }
                else
                {
                    Console.WriteLine("          активных задач нет");
                }
                TaskManager.Menu();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.ItemAdd(Biglist);
                        break;
                    case "2":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.itdone(Biglist);
                        break;
                    case "3":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.DeleteTask(Biglist);
                        break;
                    case "4":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.redact(Biglist);
                        break;
                    case "5":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.DeleteAll(Biglist);
                        break;
                    case "6":
                        Console.Clear();
                        process = false;
                        break;
                    case "7":
                        Console.Clear();
                        TaskManager.Sort(Biglist);
                        break;
                    default:
                        Console.WriteLine("перечитай инструкцию");
                        break;

                }

            }

        }
    }
