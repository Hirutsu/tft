using System.Text;
using tft;

void Task1()
{
    Console.Write("Код::");
    var code = "do while a < 5 a = a + b + c + 2 loop";
    Console.WriteLine(code);
    var analyser = new Lexical();
    analyser.Run(string.Join(Environment.NewLine, code));
    Console.WriteLine("Результат:");
    for (int i = 0; i < analyser.Lexemes.Count; i++)
    {
        Console.WriteLine($"Индекс: {i + 1}, Класс: {analyser.Lexemes[i].Class}, Тип: {analyser.Lexemes[i].Type}, Значение {analyser.Lexemes[i].Value}");
    }
}
Task1();

Console.OutputEncoding = Encoding.UTF8;
const string pathFile = @"L:\GIT\tft\tft\input.txt";




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
//TaskAvtomat();
