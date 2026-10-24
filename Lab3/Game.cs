using Lab3.Camp;
using Lab3.Combat;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.Kits;
using Lab3.World;

namespace Lab3;

public static class Game
{
    private static readonly ConsoleView View = new();

    public static void Run()
    {
        ItemCatalog catalog = DefaultCatalog.Create();
        var locations = new LocationRegistry([
            new Forest(catalog),
            new Crypt(catalog),
            new DragonLair(catalog),
        ]);
        var merchant = new Merchant(catalog, [
            DefaultCatalog.IronSword, DefaultCatalog.ElvenBow, DefaultCatalog.EmberStaff,
            DefaultCatalog.Chainmail, DefaultCatalog.PlateArmor,
        ]);

        Hero hero = CreateHero(catalog);
        Console.WriteLine($"{hero.Name} разбивает лагерь на опушке мистического леса.");

        while (true)
        {
            Console.WriteLine();
            View.PrintHero(hero);
            Console.WriteLine("1 - отправиться в экспедицию, 2 - снаряжение, 3 - торговец, 0 - выйти из игры");
            switch (ReadLine())
            {
                case "1":
                    GoOnExpedition(hero, locations);
                    break;
                case "2":
                    ManageEquipment(hero);
                    break;
                case "3":
                    VisitMerchant(hero, merchant);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Такого варианта нет");
                    break;
            }
        }
    }

    private static Hero CreateHero(ItemCatalog catalog)
    {
        Console.WriteLine("Создайте своего героя!");
        while (true)
        {
            Console.WriteLine("Выберите класс:");
            IReadOnlyList<HeroClass> heroClasses = HeroClass.Values;
            for (int i = 0; i < heroClasses.Count; i++)
            {
                IStarterKitFactory kit = heroClasses[i].GetStarterKit(catalog);
                Console.WriteLine(
                    $"{i + 1} - {heroClasses[i].DisplayName} (здоровье {heroClasses[i].BaseHealth}): " +
                    $"{View.Describe(kit.CreateWeapon())}, {View.Describe(kit.CreateArmor())}");
            }
            int classIndex = ReadChoice(heroClasses.Count);
            if (classIndex < 0)
            {
                continue;
            }
            HeroClass heroClass = heroClasses[classIndex];

            Console.WriteLine("Введите имя:");
            string name = ReadLine();

            try
            {
                return new Hero.Builder()
                    .SetName(name)
                    .SetHeroClass(heroClass)
                    .SetStarterKit(heroClass.GetStarterKit(catalog))
                    .Build();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Не получилось создать героя: " + e.Message);
            }
        }
    }

    private static void GoOnExpedition(Hero hero, LocationRegistry locations)
    {
        IReadOnlyList<Location> all = locations.All;
        Console.WriteLine("Куда отправимся?");
        for (int i = 0; i < all.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {all[i].DisplayName} (рекомендуемый уровень {all[i].RecommendedLevel})");
        }
        int index = ReadChoice(all.Count);
        if (index < 0)
        {
            return;
        }

        Location location = all[index];
        ExpeditionReport report = location.Explore(hero);
        Console.WriteLine();
        View.PrintReport(report, location.DisplayName);

        if (report.Battle.Outcome == Outcome.Victory && location.Code == DragonLair.LocationCode)
        {
            Console.WriteLine($"\nДракон повержен! Барды еще долго будут петь о подвиге героя по имени {hero.Name}.");
        }
    }

    private static void ManageEquipment(Hero hero)
    {
        // копируем список: после Equip инвентарь героя изменится
        List<Item> inventory = hero.Inventory.ToList();
        if (inventory.Count == 0)
        {
            Console.WriteLine("Инвентарь пуст");
            return;
        }

        Console.WriteLine("Что экипировать? (0 - назад)");
        PrintItems(inventory);
        int index = ReadChoice(inventory.Count);
        if (index < 0)
        {
            return;
        }

        Item item = inventory[index];
        hero.Equip(item);
        Console.WriteLine("Экипировано: " + View.Describe(item));
    }

    private static void VisitMerchant(Hero hero, Merchant merchant)
    {
        Console.WriteLine($"Торговец: \"Чего желаешь?\" (золото: {hero.Gold})");
        Console.WriteLine("1 - купить, 2 - продать, 0 - назад");
        switch (ReadLine())
        {
            case "1":
                Buy(hero, merchant);
                break;
            case "2":
                Sell(hero, merchant);
                break;
        }
    }

    private static void Buy(Hero hero, Merchant merchant)
    {
        IReadOnlyList<Item> showcase = merchant.Showcase;
        for (int i = 0; i < showcase.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {View.Describe(showcase[i])}: {showcase[i].Price} золота");
        }
        int index = ReadChoice(showcase.Count);
        if (index < 0)
        {
            return;
        }

        try
        {
            Item item = merchant.Buy(hero, showcase[index].Code);
            Console.WriteLine("Куплено: " + View.Describe(item));
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void Sell(Hero hero, Merchant merchant)
    {
        List<Item> inventory = hero.Inventory.ToList();
        if (inventory.Count == 0)
        {
            Console.WriteLine("Продавать нечего (экипированные предметы продать нельзя)");
            return;
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {View.Describe(inventory[i])}: {merchant.GetSellPrice(inventory[i])} золота");
        }
        int index = ReadChoice(inventory.Count);
        if (index < 0)
        {
            return;
        }

        Item item = inventory[index];
        int price = merchant.Sell(hero, item);
        Console.WriteLine($"Продано: {item.Name} за {price} золота");
    }

    private static void PrintItems(IReadOnlyList<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {View.Describe(items[i])}");
        }
    }

    // читает строку без пробелов по краям; если ввод закончился, завершает программу
    private static string ReadLine()
    {
        string? line = Console.ReadLine();
        if (line is null)
        {
            Environment.Exit(0);
        }

        return line.Trim();
    }

    // читает номер варианта от 1 до count, возвращает индекс или -1, если ввели что-то другое
    private static int ReadChoice(int count)
    {
        if (int.TryParse(ReadLine(), out int choice) && choice >= 1 && choice <= count)
        {
            return choice - 1;
        }

        return -1;
    }
}