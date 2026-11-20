using Lab3.Core;
using Lab3.Combat;
using Lab3.Defense;
using Lab3.Enemies;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.Tactics;
using Lab3.World;
using Lab3.Events;
using Lab3.Progress;
using Lab3.Storage;
using Lab3.GameServices;
namespace Lab3;
public static class Demo {
    private static readonly ConsoleView View = new();
    private static Hero NewHero(string name, HeroClass heroClass, int level,
                                ItemCatalog catalog) => new Hero.Builder()
                                                            .SetName(name)
                                                            .SetHeroClass(heroClass)
                                                            .SetStarterKit(heroClass.GetStarterKit(catalog))
                                                            .SetLevel(level)
                                                            .Build();
    private static void PrintHits(string title, DamageHandler chain, int count) {
        var results = new List<string>();
        for (int i = 0; i < count; i++) {
            var damage = new Damage(30);
            chain.Handle(damage);
            results.Add(damage.IsEvaded       ? "уклонение"
                        : damage.Absorbed > 0 ? $"поглощено {damage.Absorbed}, прошло {damage.Amount}"
                                              : $"прошло {damage.Amount}");
        }
        Console.WriteLine(title + ": " + string.Join("; ", results));
    }
    private sealed class EventPrinter : IGameEventListener {
        public void OnEvent(GameEvent e) => Console.WriteLine($"Событие {e.Type}: герой {e.Hero.Name}");
    }
    public static void Run() {
        var catalog = DefaultCatalog.Create();
        var dice = Dice.Instance;
        Console.WriteLine("===== Цепочка обязанностей =====");
        PrintHits("Броня 30%", new ArmorHandler(0.3), 2);
        var armor = new ArmorHandler(0.3);
        armor.SetNext(new BarrierHandler(30));
        PrintHits("Броня 30% -> барьер 30", armor, 3);
        dice.Reseed(3);
        var chain = new EvasionHandler(0.5);
        chain.SetNext(new ArmorHandler(0.3)).SetNext(new BarrierHandler(30));
        PrintHits("Уклонение 50% -> броня 30% -> барьер 30", chain, 3);
        Console.WriteLine("\n===== Стратегия =====");
        foreach (ITactic tactic in new ITactic[] { new BalancedTactic(), new AggressiveTactic(),
                                                   new CautiousTactic() }) {
            var mage = NewHero("Мерлин", HeroClass.Mage, 2, catalog);
            var report = new Crypt(catalog).Explore(mage, tactic, 9);
            string result = report.Battle.Outcome == Outcome.Victory ? "победа" : "поражение";
            Console.WriteLine(
                $"{tactic.DisplayName}: {result}, раундов {report.Battle.Events.Last().Round}, у героя осталось {mage.Health}/{mage.MaxHealth} здоровья");
        }
        Console.WriteLine("\n===== Декоратор =====");
        foreach (IEnemy enemy in new IEnemy[] {
                     new Goblin(), new LegendaryEnemy(new Goblin()), new WindfuryEnemy(new Goblin()),
                     new LegendaryEnemy(new WindfuryEnemy(new Goblin())), new ArmoredEnemy(new Goblin()),
                     new ArmoredEnemy(new LegendaryEnemy(new WindfuryEnemy(new Dragon())))
                 }) {
            dice.Reseed(5);
            string hits = string.Join(", ", Enumerable.Range(0, 3).Select(
                                                _ => enemy.RollDamage().Damage));
            var damage = enemy.ReceiveHit(20);
            Console.WriteLine(
                $"{enemy.Name}: удары {hits}, награда {enemy.ExperienceReward} опыта; входящий урон 20 -> {damage.Amount}, здоровье {enemy.Health}/{enemy.MaxHealth}");
        }
        Console.WriteLine("\n===== Адаптер =====");
        var bow = catalog.CreateWeapon(DefaultCatalog.ElvenBow);
        var haunted = new HauntedWeapon(bow);
        Console.WriteLine("Предмет: " + View.Describe(bow));
        Console.WriteLine(
            $"Враг:    {haunted.Name} ({haunted.MaxHealth} здоровья, награда {haunted.ExperienceReward} опыта)");
        var manor = new HauntedManor(catalog);
        View.PrintReport(
            manor.Explore(NewHero("Леголас", HeroClass.Ranger, 2, catalog), new CautiousTactic(), 55),
            manor.DisplayName);
        Console.WriteLine("\n===== Наблюдатель =====");
        var events = new GameEventPublisher();
        var journal = new ExpeditionJournal();
        events.Subscribe(GameEventType.Victory, new AchievementTracker());
        events.Subscribe(GameEventType.Victory, journal);
        events.Subscribe(GameEventType.Defeat, journal);
        events.Subscribe(GameEventType.Victory, new EventPrinter());
        var warrior = NewHero("Артур", HeroClass.Warrior, 1, catalog);
        var expedition = new Forest(catalog).Explore(warrior, 7);
        events.Publish(new GameEvent(expedition.Battle.Outcome == Outcome.Victory ? GameEventType.Victory
                                                                                  : GameEventType.Defeat,
                                     warrior, expedition));
        Console.WriteLine("Достижения героя: " +
                          string.Join(", ", warrior.Achievements.Select(a => a.DisplayName)));
        Console.WriteLine("Записей в журнале: " + journal.GetEntries(warrior.Name).Count);
        string directory = Path.Combine(Path.GetTempPath(), "lab-demo-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try {
            Console.WriteLine("\n===== Заместитель =====");
            string path = Path.Combine(directory, "heroes.json");
            var file = new FileHeroRepository(path, catalog);
            file.Save(warrior);
            Console.WriteLine("Содержимое файла:");
            Console.Write(File.ReadAllText(path));
            Console.WriteLine("Без заместителя два поиска дают один и тот же объект: " +
                              ReferenceEquals(file.FindByName("Артур"), file.FindByName("Артур")));
            var cached = new CachingHeroRepository(file);
            var hero = cached.FindByName("Артур")!;
            Console.WriteLine("С заместителем два поиска дают один и тот же объект: " +
                              ReferenceEquals(hero, cached.FindByName("Артур")));
            hero.AddGold(100);
            cached.Save(hero);
            Console.WriteLine("После сохранения через заместитель золото героя в файле: " +
                              file.FindByName("Артур")!.Gold);
            Console.WriteLine("\n===== Фасад =====");
            var game = new GameFacade(Path.Combine(directory, "game.json"));
            game.CreateHero("Гэндальф", "mage");
            dice.Reseed(11);
            var report = game.Explore("Гэндальф", "forest", "cautious");
            Console.WriteLine("Экспедиция через фасад: " +
                              View.Summarize(report, game.GetLocation(report.LocationCode).DisplayName,
                                             game.GetTactic(report.TacticCode).DisplayName));
            View.PrintHero(game.GetHero("Гэндальф"));
        } finally {
            Directory.Delete(directory, true);
        }
    }
}