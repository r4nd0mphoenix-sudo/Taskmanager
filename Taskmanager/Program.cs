using System.Text.Json;
class TaskItem
{
    public int number { get; set; }
    public enum PriorityLevel
    {
        High, 
        Medium, 
        Low
    }
    public PriorityLevel Priority { get; set; }
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
    public bool IsDone { get; set; }
    public string PriorityText => Priority switch
    {
        PriorityLevel.High => "высокий",
        PriorityLevel.Medium => "средний",
        PriorityLevel.Low => "низкий",
    };
    public string Writedown => $"{number}) {_name}, {PriorityText} приоритет, {(IsDone ? "[++выполнена++]":"[]")}";
}

class Logic 
{ 
    static public void NameRead(TaskItem item)
    {
        Console.WriteLine("Введите задачу");
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
    }
    static public void PriorityRead(TaskItem item)
    {
        bool work = false;
        while (!work)
        {
            Console.WriteLine("введите приоритет\n");
            Console.WriteLine(" 1 - высокий \n 2 - средний \n 3 - низкий \n");
            string input = Console.ReadLine();
            int num;
            if (int.TryParse(input, out num) && 0 < num && num <= 3)
            {
                item.Priority = (TaskItem.PriorityLevel)num - 1;
                work = true;
            }
            else
            {
                Console.WriteLine("ввод некорректен, попробуйте еще раз");
            }


        }
    }

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
                Console.WriteLine(i.Writedown);
            }
        }

    }
    static public TaskItem CreateTask(List<TaskItem> Biglist)
    {
        TaskItem item = new TaskItem();
        item.number = Biglist.Count + 1;
        return item;
    }
    static public void ItemAdd(List<TaskItem> Biglist, TaskItem item)
    {
        Logic.NameRead(item);
        Logic.PriorityRead(item);        
        item.IsDone = false;
        Biglist.Add(item);
        SaveAll(Biglist);
        Console.WriteLine(item.Writedown);
        Console.WriteLine("задача успешно сохранена\n");
    }
    static public void DeleteTask(List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер задачи, которую нужно удалить");
        bool work = true;
        while (work)
        {
            string input = Console.ReadLine();
            int num;
            if ((int.TryParse(input, out num) && num >= 1 && num <= tasks.Count))
            {
                tasks.RemoveAt(num - 1);
                for (int i = 0; i < tasks.Count; i++)
                {
                    tasks[i].number = i + 1;
                }
                SaveAll(tasks);
                work = false;
                Console.WriteLine("Задача удалена\n");
            }
            else Console.WriteLine("ввод некорректен, попробуйте еще раз");
        }
    }
    static public void EditTusk(List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер задачи, которую нужно отредактировать \n");
        bool work1 = false;
        while (!work1)
        {
            int num;
            string input = Console.ReadLine();
            if (int.TryParse(input, out num))
            {
                if (num >= 1 && num <= tasks.Count)
                {
                    Logic.NameRead(tasks[num-1]);
                    Logic.PriorityRead(tasks[num-1]);
                    SaveAll(tasks);
                    Console.WriteLine("изменения успешно сохранены\n");
                    Console.WriteLine(tasks[num-1].Writedown);
                    work1 = true;
                }
            }
            else
            {
                Console.WriteLine("ввод некорректен, попробуйте еще раз \n");
                continue;
            }

        }
    }
    static public void itIsDone(List<TaskItem> tasks)
    {
        Console.WriteLine("введите номер выполненной задачи\n");
        bool work = false;
        while (!work)
        {
            string input = Console.ReadLine();
            int num;
            if ((int.TryParse(input, out num) && num >= 1 && num <= tasks.Count))
            {
                tasks[num - 1].IsDone = true;
                work = true;
                Console.WriteLine($"задача {input} выполнена!");
                TaskManager.SaveAll(tasks);
            }
            else
            {
                Console.WriteLine("ввод некорректен, попробуйте еще раз\n");
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
    static public void FilterTask(List<TaskItem> tasks)
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
                        if (i.Priority == TaskItem.PriorityLevel.High)
                        {
                            Console.WriteLine(i.Writedown);
                        }
                    }
                    work = true;
                    Console.ReadLine();
                    break;
                case "2":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.Priority == TaskItem.PriorityLevel.Medium)
                        {
                            Console.WriteLine(i.Writedown);
                        }
                    }
                    work = true;
                    Console.ReadLine();
                    break;
                case "3":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.Priority == TaskItem.PriorityLevel.Low)
                        {
                            Console.WriteLine(i.Writedown);
                        }
                    }
                    work = true;
                    Console.ReadLine();
                    break;
                case "4":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.IsDone == true)
                        {
                            Console.WriteLine(i.Writedown);
                        }
                    }
                    work = true;
                    Console.ReadLine();
                    break;
                case "5":
                    foreach (TaskItem i in tasks)
                    {
                        if (i.IsDone == false)
                        {
                            Console.WriteLine(i.Writedown);
                        }
                    }
                    work = true;
                    Console.ReadLine();
                    break;
                default: Console.WriteLine("неправильный формат ввода, попробуйте еще раз"); input = Console.ReadLine(); break;

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
                        TaskItem item = TaskManager.CreateTask(Biglist);
                        TaskManager.ItemAdd(Biglist,item);
                        break;
                    case "2":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.itIsDone(Biglist);
                        break;
                    case "3":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.DeleteTask(Biglist);
                        break;
                    case "4":
                        Console.Clear();
                        TaskManager.ShowAll(Biglist);
                        TaskManager.EditTusk(Biglist);
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
                    TaskManager.FilterTask(Biglist);
                    break;
                default:
                        Console.WriteLine("перечитай инструкцию");
                        break;

                }

            }

        }
    }