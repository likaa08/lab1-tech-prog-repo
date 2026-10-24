namespace Lab3.Core;

// Одиночка: общий источник случайности на всю игру
public sealed class Dice
{
    // единственный экземпляр класса
    private static Dice? instance;

    private Random random = new();

    // конструктор приватный - самостоятельно создать экземпляр класса нельзя
    private Dice()
    {
    }

    // свойство для получения одиночки: оператор ??= создаст экземпляр только при первом обращении
    public static Dice Instance => instance ??= new Dice();

    // после установки сида все последующие броски повторяются в точности
    public void Reseed(int seed)
    {
        random = new Random(seed);
    }

    // случайный сид
    public int NextSeed()
    {
        return random.Next(1_000_000);
    }

    // случайное целое число от min до max включительно
    public int Roll(int min, int max)
    {
        return random.Next(min, max + 1);
    }

    // возвращает true с вероятностью probability (от 0 до 1)
    public bool Chance(double probability)
    {
        return random.NextDouble() < probability;
    }

    // случайный элемент списка
    public T Pick<T>(IReadOnlyList<T> options)
    {
        return options[random.Next(options.Count)];
    }
}