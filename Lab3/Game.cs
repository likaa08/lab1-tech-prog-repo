using Lab3.GameServices;
using Lab3.Camp;
using Lab3.Combat;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.Kits;
using Lab3.World;

namespace Lab3;

public static class Game {
    private static readonly ConsoleView View = new();

    public static void Run() {
        string saveFile = Path.Combine("saves", "heroes.json");
        Console.WriteLine("Файл сохранения: " + Path.GetFullPath(saveFile));
        var game = new GameFacade(saveFile);
        Hero hero = ChooseHero(game);
        Console.WriteLine($"{hero.Name} разбивает лагерь на опушке мистического леса.");

        while (true) {
            Console.WriteLine();
            View.PrintHero(hero);
            Console.WriteLine(
                "1 - отправиться в экспедицию, 2 - снаряжение, 3 - торговец, 4 - журнал, 0 - выйти из игры");
            switch (ReadLine()) {
            case "1":
                GoOnExpedition(hero, game);
                break;
            case "2":
                ManageEquipment(hero, game);
                break;
            case "3":
                VisitMerchant(hero, game);
                break;
            case "4":
                foreach (var report in game.GetJournal(hero.Name))
                    Console.WriteLine(View.Summarize(report,
                                                     game.GetLocation(report.LocationCode).DisplayName,
                                                     game.GetTactic(report.TacticCode).DisplayName));
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Такого варианта нет");
                break;
            }
        }
    }

    private static Hero ChooseHero(GameFacade game) {
        var heroes = game.Heroes;
        if (heroes.Count == 0)
            return CreateHero(game);
        Console.WriteLine("Выберите сохраненного героя (0 - создать нового):");
        for (int i = 0; i < heroes.Count; i++)
            Console.WriteLine(
                $"{i + 1} - {heroes[i].Name}, {heroes[i].HeroClass.DisplayName} {heroes[i].Level} ур.");
        int index = ReadChoice(heroes.Count);
        return index >= 0 ? heroes[index] : CreateHero(game);
    }

    private static Hero CreateHero(GameFacade game) {
        Console.WriteLine("Создайте своего героя!");
        while (true) {
            Console.WriteLine("Выберите класс:");
            IReadOnlyList<HeroClass> heroClasses = HeroClass.Values;
            for (int i = 0; i < heroClasses.Count; i++) {
                var kit = game.GetStarterKit(heroClasses[i].Code);
                Console.WriteLine(
                    $"{i + 1} - {heroClasses[i].DisplayName} (здоровье {heroClasses[i].BaseHealth}): " +
                    $"{View.Describe(kit[0])}, {View.Describe(kit[1])}");
            }
            int classIndex = ReadChoice(heroClasses.Count);
            if (classIndex < 0) {
                continue;
            }
            HeroClass heroClass = heroClasses[classIndex];

            Console.WriteLine("Введите имя:");
            string name = ReadLine();

            try {
                return game.CreateHero(name, heroClass.Code);
            } catch (ArgumentException e) {
                Console.WriteLine("Не получилось создать героя: " + e.Message);
            }
        }
    }

    private static void GoOnExpedition(Hero hero, GameFacade game) {
        IReadOnlyList<Location> all = game.Locations;
        Console.WriteLine("Куда отправимся?");
        for (int i = 0; i < all.Count; i++) {
            Console.WriteLine(
                $"{i + 1} - {all[i].DisplayName} (рекомендуемый уровень {all[i].RecommendedLevel})");
        }
        int index = ReadChoice(all.Count);
        if (index < 0) {
            return;
        }

        Location location = all[index];
        Console.WriteLine("Выберите тактику:");
        for (int i = 0; i < game.Tactics.Count; i++)
            Console.WriteLine($"{i + 1} - {game.Tactics[i].DisplayName}: {game.Tactics[i].Description}");
        int tacticIndex = ReadChoice(game.Tactics.Count);
        if (tacticIndex < 0)
            return;
        ExpeditionReport report = game.Explore(hero.Name, location.Code, game.Tactics[tacticIndex].Code);
        Console.WriteLine();
        View.PrintReport(report, location.DisplayName);

        if (report.Battle.Outcome == Outcome.Victory && report.Battle.Enemy.Code == "dragon") {
            Console.WriteLine(
                $"\nДракон повержен! Барды еще долго будут петь о подвиге героя по имени {hero.Name}.");
        }
    }

    private static void ManageEquipment(Hero hero, GameFacade game) {
        // копируем список: после Equip инвентарь героя изменится
        List<Item> inventory = hero.Inventory.ToList();
        if (inventory.Count == 0) {
            Console.WriteLine("Инвентарь пуст");
            return;
        }

        Console.WriteLine("Что экипировать? (0 - назад)");
        PrintItems(inventory);
        int index = ReadChoice(inventory.Count);
        if (index < 0) {
            return;
        }

        Item item = inventory[index];
        game.Equip(hero.Name, index);
        Console.WriteLine("Экипировано: " + View.Describe(item));
    }

    private static void VisitMerchant(Hero hero, GameFacade game) {
        Console.WriteLine($"Торговец: \"Чего желаешь?\" (золото: {hero.Gold})");
        Console.WriteLine("1 - купить, 2 - продать, 0 - назад");
        switch (ReadLine()) {
        case "1":
            Buy(hero, game);
            break;
        case "2":
            Sell(hero, game);
            break;
        }
    }

    private static void Buy(Hero hero, GameFacade game) {
        IReadOnlyList<Item> showcase = game.Showcase;
        for (int i = 0; i < showcase.Count; i++) {
            Console.WriteLine($"{i + 1} - {View.Describe(showcase[i])}: {showcase[i].Price} золота");
        }
        int index = ReadChoice(showcase.Count);
        if (index < 0) {
            return;
        }

        try {
            Item item = game.Buy(hero.Name, showcase[index].Code);
            Console.WriteLine("Куплено: " + View.Describe(item));
        } catch (InvalidOperationException e) {
            Console.WriteLine(e.Message);
        }
    }

    private static void Sell(Hero hero, GameFacade game) {
        List<Item> inventory = hero.Inventory.ToList();
        if (inventory.Count == 0) {
            Console.WriteLine("Продавать нечего (экипированные предметы продать нельзя)");
            return;
        }

        for (int i = 0; i < inventory.Count; i++) {
            Console.WriteLine(
                $"{i + 1} - {View.Describe(inventory[i])}: {game.GetSellPrice(inventory[i])} золота");
        }
        int index = ReadChoice(inventory.Count);
        if (index < 0) {
            return;
        }

        Item item = inventory[index];
        int price = game.Sell(hero.Name, index);
        Console.WriteLine($"Продано: {item.Name} за {price} золота");
    }

    private static void PrintItems(IReadOnlyList<Item> items) {
        for (int i = 0; i < items.Count; i++) {
            Console.WriteLine($"{i + 1} - {View.Describe(items[i])}");
        }
    }

    // читает строку без пробелов по краям; если ввод закончился, завершает программу
    private static string ReadLine() {
        string? line = Console.ReadLine();
        if (line is null) {
            Environment.Exit(0);
        }

        return line.Trim();
    }

    // читает номер варианта от 1 до count, возвращает индекс или -1, если ввели что-то другое
    private static int ReadChoice(int count) {
        if (int.TryParse(ReadLine(), out int choice) && choice >= 1 && choice <= count) {
            return choice - 1;
        }

        return -1;
    }
}