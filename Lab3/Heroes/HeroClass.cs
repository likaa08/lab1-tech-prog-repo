using Lab3.Defense;
using Lab3.Core;
using Lab3.Items;
using Lab3.Kits;

namespace Lab3.Heroes;

// перечисление (enum) в C# не может хранить данные, поэтому классы героя - это
// набор заранее созданных объектов, а создать новые снаружи нельзя: конструктор приватный
public sealed class HeroClass {
    // лямбда - функция, которая создает фабрику стартового снаряжения
    public static readonly HeroClass Warrior =
        new("warrior", "Воин", 120, catalog => new WarriorKitFactory(catalog));
    public static readonly HeroClass Ranger =
        new("ranger", "Следопыт", 100, catalog => new RangerKitFactory(catalog));
    public static readonly HeroClass Mage = new("mage", "Маг", 85, catalog => new MageKitFactory(catalog));

    // все классы героя по порядку
    public static readonly IReadOnlyList<HeroClass> Values = [Warrior, Ranger, Mage];

    private readonly Func<ItemCatalog, IStarterKitFactory> starterKit;

    private HeroClass(string code, string displayName, int baseHealth,
                      Func<ItemCatalog, IStarterKitFactory> starterKit) {
        Code = code;
        DisplayName = displayName;
        BaseHealth = baseHealth;
        this.starterKit = starterKit;
    }

    public string Code { get; }

    public string DisplayName { get; }

    public int BaseHealth { get; }

    public static HeroClass ByCode(string code) {
        return Values.FirstOrDefault(heroClass => heroClass.Code == code) ??
               throw new UnknownCodeException("Класс героя", code);
    }

    // каждый класс сам знает свою фабрику стартового снаряжения
    public IStarterKitFactory GetStarterKit(ItemCatalog catalog) {
        return starterKit(catalog);
    }
    public DamageHandler CreateDefense(Armor armor, int level) {
        double evasion = this == Ranger ? 0.15 : 0;
        int barrier = this == Mage ? 30 : 0;
        if (this == Warrior && level >= 3)
            barrier = 20;
        else if (this == Ranger && level >= 5)
            barrier = 15;
        else if (this == Mage && level >= 3)
            evasion = 0.10;
        DamageHandler armorHandler = new ArmorHandler(armor.Defense), first = armorHandler;
        if (evasion > 0) {
            first = new EvasionHandler(evasion);
            first.SetNext(armorHandler);
        }
        if (barrier > 0)
            armorHandler.SetNext(new BarrierHandler(barrier));
        return first;
    }
}