using DeliveryService;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        List<Service> services = InputData();
        bool flag = true;

        while (flag)
        {
            Console.WriteLine("Меню выбора:");
            Console.WriteLine("1. Ввод дополнительных данных\n2. Фильтрация\n3. Вывести текущие данные\n4. Удалить текущие данные и ввод новых\n5. Выход из программы");
            Console.WriteLine("Введите номер операции");
            int operation = Convert.ToInt32(Console.ReadLine());
            switch (operation)
            {
                case 1:
                    var servicesAdditional = InputData();
                    services.AddRange(servicesAdditional);
                    break;
                case 2:
                    Filtration(services);
                    break;
                case 3:
                    foreach (var service in services) Console.WriteLine(service.ToString());
                    break;
                case 4:
                    services.Clear();
                    Console.WriteLine("Текущие данные удалены");
                    services = InputData();
                    break;
                case 5:
                    flag = false;
                    break;
                default:
                    Console.WriteLine("Такой операции нету!");
                    break;
            }
        }
    }

    // Ввод данных
    static List<Service> InputData()
    {
        Console.WriteLine("Введите названия файла или путь к файлу: ");
        string? name = Console.ReadLine();
        // в ResultInput будут записаны данные, которые прошли валидацию
        List<Service> ResultInput = new();
        // в ErrorList записываются номера заказов, в которых возникает ошибка
        List<string> ErrorList = new();
        while (true) {
            if (File.Exists(name))
            {
                using (StreamReader reader = new StreamReader(name))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] str = line.Split(' ');

                        if (str.GetLength(0) == 5 && Validator(str))
                        {
                            ResultInput.Add(new Service(str[0], Convert.ToDouble(str[1].Replace('.', ',')), str[2], Convert.ToDateTime(str[3] + " " + str[4])));
                        }
                        else
                        {
                            ErrorList.Add(str[0]);
                        }
                    }
                }
                break;
            }
            else
            {
                Console.WriteLine("Такого файла не существует, введите название файла, если хотите выйти, то напишите 'Exit'");
                name = Console.ReadLine();
                if (name == "Exit") break;
            }
        }
        if (ErrorList.Count > 0)
        {
            Console.WriteLine("Номера заказов, которые не были добавлены в список из-за неправильного формата ввода");
            foreach (var err in ErrorList) Console.WriteLine(err);
        }
        return ResultInput;
    }

    // Валидатор
    static bool Validator(string[] str)
    {
        try
        {
            // В основном ошибки в записи могут возникнут в данных переменных, с номером заказа точно не знаю, будут ли использоваться буквы с числами или только числа, но на всякий случай добавил проверку
            // В случае, если вы будете использовать номера с буквами и числами, прошу убрать строку id
            int id = Convert.ToInt32(str[0]);
            double weight = Convert.ToDouble(str[1].Replace('.', ','));
            DateTime date = Convert.ToDateTime(str[3]);
        }
        catch (FormatException)
        {
            return false;
        }
        return true;
    }

    // Фильтрация
    static void Filtration(List<Service> services)
    {
        var time = services[0].DeliveryTime;

        Console.WriteLine("Введите район для фильтрации: ");
        var districtFilter = Console.ReadLine();

        // Использовал Linq для фильтрации

        var result = from service in services
                 where service.Disctrict == districtFilter && service.DeliveryTime >= time && time.AddMinutes(30) >= service.DeliveryTime
                 orderby service.DeliveryTime
                 select service;

        Console.WriteLine("Фильтрация завершена");
        Output(result);
    }

    // Общий вывод
    static void Output(IOrderedEnumerable<Service> result)
    {
        if (result.Count() > 0)
        {
            Console.WriteLine("Выберите способ вывода:\n1. Вывод в консоль\n2. Вывод в файл");
            int operation = Convert.ToInt32(Console.ReadLine());

            switch (operation)
            {
                case 1:
                    foreach (var service in result) Console.WriteLine(service.ToString());
                    break;
                case 2:
                    OutputFileResult(result);
                    break;
                default:
                    Console.WriteLine("Такого действия нет!");
                    break;
            }
        }
        else Console.WriteLine("Данных после фильтрации нет.");
    }

    // Вывод в файл
    static void OutputFileResult(IOrderedEnumerable<Service> services)
    {
        Console.WriteLine("Введите название файла, куда будет происходить запись, либо путь к файлу, либо название для нового файла: ");
        string? file = Console.ReadLine();
        Console.WriteLine("Если в данном файле есть данные, то выберите одно из действий (в случае нового файла, выбрать можно любой):\n1. Перезапись данные\n2. Добавление данных");
        bool append = Console.ReadLine() == "1" ? false : true;

        if(file != null)
        {
            using (StreamWriter writer = new StreamWriter(file, append))
            {
                foreach (var service in services)
                    writer.WriteLine(service.ToString());
            }
        }
        Console.WriteLine($"Данные были {(!append ? "изменены в файле" : "добавлены в файл")} '{file}'");
    }
}