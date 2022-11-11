using System.Text;
using tft;

Console.OutputEncoding = Encoding.UTF8;
const string pathFile = @"L:\GIT\tft\tft\input.txt";
const string codeTask1 = "do while a < 5 a = a + b + c + 2 loop";

List<string> codeTask2 = new()
{
    "do while a < b",
    "e = 6 + g",
    "z = x - 14",
    "output e",
    "loop"
};
List<string> codeTask3 = new()
{
    "do while a >= 20",
        "	a = 12",
        "	a = a + 5",
    "	output a",
    "loop"
};
using var stream = new StreamReader(@"L:\GIT\tft\tft\InputFiles\codeStringForTask4.txt");
var codeTask4 = stream.ReadToEnd();

void Task1()
{
    Console.Write("Код::");
    Console.WriteLine(codeTask1);
    var analyser = new Lexical();
    analyser.Run(string.Join(Environment.NewLine, codeTask1));
    Console.WriteLine("Результат:");
    for (int i = 0; i < analyser.Lexemes.Count; i++)
    {
        Console.WriteLine($"Индекс: {i + 1}, Класс: {analyser.Lexemes[i].Class}, Тип: {analyser.Lexemes[i].Type}, Значение {analyser.Lexemes[i].Value}");
    }
}

void Task2()
{
    var analyser = new SyntaxAnalyzer();
    try
    {
        var result = analyser.Run(string.Join(Environment.NewLine, codeTask2));
        Console.WriteLine("Результат: ");
        Console.WriteLine(result ? "Все прошло успешно" : "Неподходящая конструкция");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void Task3()
{
    var analyser = new AnalyzerPOLIZ();
    try
    {
        var result = analyser.Run(string.Join(Environment.NewLine, codeTask3), out List<Entry> entryList);
        Console.Write("Результат: ");
        Console.WriteLine(result ? "Все прошло успешно" : "Неподходящая конструкция");
        foreach (var entry in entryList)
        {
            if (entry.EntryType == EntryType.Var) FormatOut(entry.Value);
            else if (entry.EntryType == EntryType.Const) FormatOut(entry.Value);
            else if (entry.EntryType == EntryType.Cmd) FormatOut(entry.Cmd.ToString());
            else if (entry.EntryType == EntryType.CmdPtr) FormatOut($"{entry.CmdPtr}");
        }
        Console.WriteLine();
        for (int i = 0; i < entryList.Count + 1; i++)
        {
            FormatOut($"{i}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine();
}

void Task4()
{
    Console.WriteLine("Код в файле:");
    Console.WriteLine(codeTask4);

    Interpreter interpreter = new();
    try
    {
        interpreter.Run(codeTask4);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void TaskAvtomat()
{
    Console.WriteLine("Автомат который имеет пути");
    var automatDeterminateWays = new Dictionary<string, Dictionary<char, List<string>>>()
    {
        {"S1" , new Dictionary<char, List<string>>()
            {
                {'a', new List<string>(){"S1"}},
                {'b', new List<string>(){"S2"}},
                {'c', new List<string>(){"S3"}},
            }
        },
        {"S2" , new Dictionary<char, List<string>>()
            {
                {'a', new List<string>(){"S2"}},
                {'b', new List<string>(){"S3"}},
                {'c', new List<string>(){"S4"}}
            }
        },
        {"S3" , new Dictionary<char, List<string>>()
            {
                {'a', new List<string>(){"S3"}},
                {'b', new List<string>(){"S4"}},
                {'c', new List<string>(){"S1"}}
            }
        },
        {"S4" , new Dictionary<char, List<string>>()
            {
                {'a', new List<string>(){"S4"}},
                {'b', new List<string>(){"S1"}},
                {'c', new List<string>(){"S2"}}
            }
        }
    };

    var deterAuto = new Automat<string, char>("S1", new List<string>() { "S3" }, automatDeterminateWays);
    string deterString = "baacbbba";
    if (deterAuto.Run(deterString, out _))
    {
        Console.WriteLine("Подходит");
    }
    Console.WriteLine("\n\n");


    var automatNonDeterminateWays = new Dictionary<string, Dictionary<string, List<string>>>()
    {
        {"S1" , new Dictionary<string, List<string>>()
            {
                {"a", new List<string>(){"S1", "S3"}},
                {"b", new List<string>(){"S2"}},
            }
        },
        {"S2" , new Dictionary<string, List<string>>()
            {
                {"a", new List<string>(){"S4"}},
                {"b", new List<string>(){"S2"}},
            }
        },
        {"S3" , new Dictionary<string, List<string>>()
            {
                {"a", new List<string>(){"S1"}},
                {"b", new List<string>(){"S2"}},
            }
        },
        {"S4" , new Dictionary<string, List<string>>()
            {
                {"a", new List<string>(){"S4"}},
                {"b", new List<string>(){"S4", "S2"}},
            }
        }
    };

    Console.WriteLine("Автомат с детерменированным");
    var nonDeterAuto = new Automat<string, string>("S1", new List<string>() { "S2", "S4" }, automatNonDeterminateWays, StatesQueueOptions.UnicWays);
    string nonDeterString = "babab";
    if (nonDeterAuto.Run(nonDeterString, out _))
    {
        Console.WriteLine("Подходит");
    }
    Console.WriteLine();
    nonDeterAuto.WorkOption = StatesQueueOptions.AllWays;
    if (nonDeterAuto.Run(nonDeterString))
    {
        Console.WriteLine("Подходит");
    }
    Console.WriteLine("\n\n");

    var nonDeterEpsAuto = new Automat<string, string>("S1", new List<string>() { "S3", "S4" }, "EPSILON", pathFile, StatesQueueOptions.UnicWays);
    string nonDeterEpsString = "ab";
    if (nonDeterEpsAuto.Run(nonDeterEpsString))
    {
        Console.WriteLine("Подходит");
    }
    Console.WriteLine();
    nonDeterEpsAuto.WorkOption = StatesQueueOptions.AllWays;
    if (nonDeterEpsAuto.Run(nonDeterEpsString))
    {
        Console.WriteLine("Подходит");
    }

    Console.WriteLine("Press any key to exit");
    Console.ReadLine();
}

void FormatOut(string str)
{
    Console.Write("{0, 6} ", str);
}

Console.WriteLine("Меню");
Console.WriteLine("1 - 1ая Лабораторная работа");
Console.WriteLine("2 - 2ая Лабораторная работа");
Console.WriteLine("3 - 3ая Лабораторная работа");
Console.WriteLine("4 - 4ая Лабораторная работа");
Console.WriteLine("5 - Автомат");

int menu = Convert.ToInt32(Console.ReadLine());
switch (menu)
{
    case 1:
        Task1();
        break;
    case 2:
        Task2();
        break;
    case 3:
        Task3();
        break;
    case 4:
        Task4();
        break;
    case 6:
        TaskAvtomat();
        break;
    default:
        break;
}
