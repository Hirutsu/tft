// See https://aka.ms/new-console-template for more information
using tft;

Avtomat avtomat = new Avtomat(@"L:\GIT\tft\tft\input.txt");

Console.WriteLine("Write sequence:");
string input = Console.ReadLine();
avtomat.Passage(input);

Console.WriteLine();
avtomat.ShowLstVertex();
