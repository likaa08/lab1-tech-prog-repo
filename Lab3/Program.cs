using System.Text;
using Lab3;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// точка входа: без аргументов запускается игра, с аргументом demo - демонстрация паттернов
if (args.Length > 0 && args[0] == "demo")
{
    Demo.Run();
}
else
{
    Game.Run();
}