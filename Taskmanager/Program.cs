using System;

class Task
{
    public string Description;
    public string Priority;
    public int Number;


    public Task(string description, string priority,int number)
    {
        Description = description;
        Priority = priority;
        Number = number;
    }

    public void Writedown()
    {
        Console.WriteLine($"задача номер {Number}, {Description}, с приоритетом:{Priority}.");
    }

}

class Program
{
    static void Main()
    {
        int n = 1;
        List<Task> Tasklist = new List<Task>();
        while (true)
        {
            Console.WriteLine("Введите описание задачи:");
            string description = Console.ReadLine();
            if (description == "")
            {
                Console.WriteLine("если хотите выйти - нажмите Enter еще раз");
                description = Console.ReadLine();
                if (description == "")
                {
                    break;
                }
            }

            Console.WriteLine("Введите приоритет задачи (средний/низкий/высокий):");
            string priority = Console.ReadLine();
            if (priority == "")
            {
                Console.WriteLine("если хотите выйти - нажмите Enter еще раз");
                priority = Console.ReadLine();
                if (priority == "")
                {
                    break;
                }
            }
            if ((priority=="высокий" || priority=="средний" || priority== "низкий")==false)
            {
                Console.WriteLine("Неправильный формат ввода, попробуйте еще раз");
                priority = Console.ReadLine();
            }
            Task Task1=new Task(description,priority,n++);
            Tasklist.Add(Task1);            
            foreach (Task Task in Tasklist) Task.Writedown();
        }
    }
}
