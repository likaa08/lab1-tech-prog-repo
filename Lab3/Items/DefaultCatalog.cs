namespace Lab3.Items;

// все предметы, которые есть в игре
public static class DefaultCatalog
{
    // коды шаблонов: по ним предметы ищутся в каталоге
    // стартовое снаряжение
    public const string TrainingSword = "training-sword";
    public const string HuntingBow = "hunting-bow";
    public const string ApprenticeStaff = "apprentice-staff";
    public const string Chainmail = "chainmail";
    public const string LeatherArmor = "leather-armor";
    public const string LinenRobe = "linen-robe";

    // товары торговца
    public const string IronSword = "iron-sword";
    public const string ElvenBow = "elven-bow";
    public const string EmberStaff = "ember-staff";
    public const string PlateArmor = "plate-armor";

    // добыча
    public const string GoblinCleaver = "goblin-cleaver";
    public const string BoneBow = "bone-bow";
    public const string NecromancerStaff = "necromancer-staff";
    public const string KnightBlade = "knight-blade";
    public const string CryptMail = "crypt-mail";
    public const string DawnBlade = "dawn-blade";
    public const string DragonscaleArmor = "dragonscale-armor";

    public static ItemCatalog Create()
    {
        var catalog = new ItemCatalog();

        // стартовое снаряжение
        catalog.Register(new Sword(TrainingSword, "Тренировочный меч", 20, 12));
        catalog.Register(new Bow(HuntingBow, "Охотничий лук", 20, 10, 0.3));
        catalog.Register(new Staff(ApprenticeStaff, "Посох ученика", 20, 14, 0.3));
        catalog.Register(new Armor(Chainmail, "Кольчуга", 40, 0.3));
        catalog.Register(new Armor(LeatherArmor, "Кожаная броня", 30, 0.2));
        catalog.Register(new Armor(LinenRobe, "Льняная роба", 15, 0.1));

        // товары торговца
        catalog.Register(new Sword(IronSword, "Железный меч", 80, 17));
        catalog.Register(new Bow(ElvenBow, "Эльфийский лук", 90, 14, 0.4));
        catalog.Register(new Staff(EmberStaff, "Тлеющий посох", 90, 20, 0.3));
        catalog.Register(new Armor(PlateArmor, "Латы", 120, 0.4));

        // добыча
        catalog.Register(new Sword(GoblinCleaver, "Гоблинский тесак", 40, 15));
        catalog.Register(new Bow(BoneBow, "Костяной лук", 140, 17, 0.4));
        catalog.Register(new Staff(NecromancerStaff, "Посох некроманта", 150, 25, 0.35));
        catalog.Register(new Sword(KnightBlade, "Клинок рыцаря", 150, 23));
        catalog.Register(new Armor(CryptMail, "Кольчуга стража склепа", 160, 0.45));
        catalog.Register(new Sword(DawnBlade, "Клинок рассвета", 500, 35));
        catalog.Register(new Armor(DragonscaleArmor, "Драконья чешуя", 500, 0.55));

        return catalog;
    }
}