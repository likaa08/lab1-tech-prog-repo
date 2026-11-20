using Lab3.Camp;
using Lab3.Combat;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.World;
using Lab3.Tactics;
using Lab3.Events;
using Lab3.Progress;
using Lab3.Storage;
namespace Lab3.GameServices;
public sealed class GameFacade {
    private readonly ItemCatalog catalog = DefaultCatalog.Create();
    private readonly LocationRegistry registry;
    private readonly Merchant merchant;
    private readonly IHeroRepository heroes;
    private readonly GameEventPublisher events = new();
    private readonly ExpeditionJournal journal = new();
    public IReadOnlyList<ITactic> Tactics { get; } =
        [new BalancedTactic(), new AggressiveTactic(), new CautiousTactic()];
    public GameFacade(string saveFile) {
        registry = new LocationRegistry(
            [new Forest(catalog), new HauntedManor(catalog), new Crypt(catalog), new DragonLair(catalog)]);
        merchant = new Merchant(catalog, [
            DefaultCatalog.IronSword, DefaultCatalog.ElvenBow, DefaultCatalog.EmberStaff,
            DefaultCatalog.Chainmail, DefaultCatalog.PlateArmor
        ]);
        heroes = new CachingHeroRepository(new FileHeroRepository(saveFile, catalog));
        var achievements = new AchievementTracker();
        events.Subscribe(GameEventType.Victory, achievements);
        events.Subscribe(GameEventType.LevelUp, achievements);
        events.Subscribe(GameEventType.Victory, journal);
        events.Subscribe(GameEventType.Defeat, journal);
    }
    public IReadOnlyList<HeroClass> HeroClasses => HeroClass.Values;
    public IReadOnlyList<Item> GetStarterKit(string code) {
        var kit = HeroClass.ByCode(code).GetStarterKit(catalog);
        return [kit.CreateWeapon(), kit.CreateArmor()];
    }
    public IReadOnlyList<Location> Locations => registry.All;
    public Location GetLocation(string code) => registry.Get(code);
    public ITactic GetTactic(string code) => Tactics.FirstOrDefault(t => t.Code == code) ??
                                             throw new ArgumentException("Неизвестная тактика: " + code);
    public IReadOnlyList<Item> Showcase => merchant.Showcase;
    public int GetSellPrice(Item item) => merchant.GetSellPrice(item);
    public IReadOnlyList<Hero> Heroes => heroes.FindAll();
    public Hero GetHero(string name) => heroes.FindByName(name) ??
                                        throw new ArgumentException("Героя с именем " + name + " нет");
    public Hero CreateHero(string name, string code) {
        if (heroes.FindByName(name) is not null)
            throw new ArgumentException("Герой с именем " + name + " уже есть");
        var heroClass = HeroClass.ByCode(code);
        var hero = new Hero.Builder()
                       .SetName(name)
                       .SetHeroClass(heroClass)
                       .SetStarterKit(heroClass.GetStarterKit(catalog))
                       .Build();
        heroes.Save(hero);
        return hero;
    }
    public ExpeditionReport Explore(string name, string locationCode, string tacticCode) {
        var hero = GetHero(name);
        var report = GetLocation(locationCode).Explore(hero, GetTactic(tacticCode));
        var result = report.Battle.Outcome == Outcome.Victory ? GameEventType.Victory : GameEventType.Defeat;
        events.Publish(new GameEvent(result, hero, report));
        if (report.Rewards.LevelsGained > 0)
            events.Publish(new GameEvent(GameEventType.LevelUp, hero, report));
        heroes.Save(hero);
        return report;
    }
    public Item Equip(string name, int index) {
        var hero = GetHero(name);
        var item = hero.Inventory[index];
        hero.Equip(item);
        heroes.Save(hero);
        return item;
    }
    public Item Buy(string name, string code) {
        var hero = GetHero(name);
        var item = merchant.Buy(hero, code);
        heroes.Save(hero);
        return item;
    }
    public int Sell(string name, int index) {
        var hero = GetHero(name);
        int price = merchant.Sell(hero, hero.Inventory[index]);
        heroes.Save(hero);
        return price;
    }
    public IReadOnlyList<ExpeditionReport> GetJournal(string name) => journal.GetEntries(name);
}