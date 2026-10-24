using Lab3.Core;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.Kits;
using Lab3.World;

namespace Lab3;

public static class Demo
{
    public static void Run()
    {
        ItemCatalog catalog = DefaultCatalog.Create();
        var view = new ConsoleView();

        Console.WriteLine("===== Одиночка =====");
        Dice first = Dice.Instance;
        Dice second = Dice.Instance;
        Console.WriteLine($"Dice.Instance возвращает один и тот же объект: {ReferenceEquals(first, second)}");

        Console.WriteLine();
        Console.WriteLine("===== Прототип =====");
        Item plainSword = catalog.Create(DefaultCatalog.IronSword);
        Item fierySword = catalog.Create(DefaultCatalog.IronSword);
        fierySword.Enchant(Enchantment.Fiery);
        Console.WriteLine("Первая копия:        " + view.Describe(plainSword));
        Console.WriteLine("Вторая копия:        " + view.Describe(fierySword));
        Console.WriteLine("Новая копия шаблона: " + view.Describe(catalog.Create(DefaultCatalog.IronSword)));
        Console.WriteLine($"Копии - разные объекты: {!ReferenceEquals(plainSword, fierySword)}");

        Console.WriteLine();
        Console.WriteLine("===== Абстрактная фабрика =====");
        foreach (HeroClass heroClass in HeroClass.Values)
        {
            IStarterKitFactory kit = heroClass.GetStarterKit(catalog);
            Console.WriteLine($"{heroClass.DisplayName}: {view.Describe(kit.CreateWeapon())}, {view.Describe(kit.CreateArmor())}");
        }

        Console.WriteLine();
        Console.WriteLine("===== Строитель =====");
        Hero hero = new Hero.Builder()
            .SetName("Артур")
            .SetHeroClass(HeroClass.Warrior)
            .SetStarterKit(HeroClass.Warrior.GetStarterKit(catalog))
            .SetLevel(3)
            .Build();
        view.PrintHero(hero);
        try
        {
            new Hero.Builder()
                .SetHeroClass(HeroClass.Mage)
                .SetStarterKit(HeroClass.Mage.GetStarterKit(catalog))
                .Build();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Герой без имени не создается: " + e.Message);
        }
        try
        {
            new Hero.Builder()
                .SetName("Мерлин")
                .Build();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Герой без класса не создается: " + e.Message);
        }

        Console.WriteLine();
        Console.WriteLine("===== Фабричный метод =====");
        var forest = new Forest(catalog);
        var crypt = new Crypt(catalog);
        view.PrintReport(forest.Explore(hero, 7), forest.DisplayName);
        Console.WriteLine();
        view.PrintReport(crypt.Explore(hero, 7), crypt.DisplayName);
    }
}