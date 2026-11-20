using Lab3.Defense;
using Lab3.Progress;
using Lab3.Combat;
using Lab3.Items;
using Lab3.Kits;

namespace Lab3.Heroes;

public sealed class Hero : ICombatant {
    private const int MaxNameLength = 20;
    private const int StartingGold = 50;
    private const int HealthPerLevel = 15;
    private const int DamagePerLevel = 3;
    private const int ExperiencePerLevel = 100;

    private readonly List<Item> inventory = new();
    private readonly List<Achievement> achievements;
    private DamageHandler defense = null!;
    public IReadOnlyList<Achievement> Achievements => achievements.AsReadOnly();

    // конструктор приватный и принимает в себя билдера, из которого и берет значения полей
    private Hero(Builder builder) {
        Name = builder.Name!;
        HeroClass = builder.HeroClass!;
        Level = builder.Level;
        Gold = builder.Gold;
        Weapon = builder.Weapon!;
        Armor = builder.Armor!;
        Experience = builder.Experience;
        inventory.AddRange(builder.Inventory);
        achievements = builder.Achievements.Distinct().ToList();
        Rest();
    }

    // внутренний класс билдер - чтобы удобно и безопасно создавать героев
    public sealed class Builder {
        internal string? Name { get; private set; }

        internal HeroClass? HeroClass { get; private set; }

        internal Weapon? Weapon { get; private set; }

        internal Armor? Armor { get; private set; }

        // необязательные поля сразу получают значения по умолчанию
        internal int Level { get; private set; } = 1;

        internal int Gold { get; private set; } = StartingGold;

        internal int Experience { get; private set; }
        internal List<Item> Inventory { get; private set; } = new();
        internal List<Achievement> Achievements { get; private set; } = new();
        public Builder SetExperience(int value) {
            Experience = value;
            return this;
        }
        public Builder SetInventory(IEnumerable<Item> items) {
            Inventory = items.ToList();
            return this;
        }
        public Builder SetAchievements(IEnumerable<Achievement> items) {
            Achievements = items.ToList();
            return this;
        }

        public Builder SetName(string name) {
            Name = name;
            return this;
        }

        public Builder SetHeroClass(HeroClass heroClass) {
            HeroClass = heroClass;
            return this;
        }

        public Builder SetWeapon(Weapon weapon) {
            Weapon = weapon;
            return this;
        }

        public Builder SetArmor(Armor armor) {
            Armor = armor;
            return this;
        }

        // берет оружие и броню из стартового набора
        public Builder SetStarterKit(IStarterKitFactory starterKit) {
            Weapon = starterKit.CreateWeapon();
            Armor = starterKit.CreateArmor();
            return this;
        }

        public Builder SetLevel(int level) {
            Level = level;
            return this;
        }

        public Builder SetGold(int gold) {
            Gold = gold;
            return this;
        }

        // перед созданием героя проверяем, что все обязательные поля заполнены корректно
        public Hero Build() {
            if (string.IsNullOrWhiteSpace(Name)) {
                throw new ArgumentException("У героя должно быть имя");
            }
            if (Name.Length > MaxNameLength) {
                throw new ArgumentException($"Имя героя не может быть длиннее {MaxNameLength} символов");
            }
            if (HeroClass is null) {
                throw new ArgumentException("Не выбран класс героя");
            }
            if (Weapon is null || Armor is null) {
                throw new ArgumentException("Герою нужны оружие и броня");
            }
            if (Level < 1) {
                throw new ArgumentException("Уровень героя не может быть меньше 1");
            }
            if (Gold < 0) {
                throw new ArgumentException("Золота не может быть меньше нуля");
            }

            if (Experience < 0)
                throw new ArgumentException("Опыт не может быть отрицательным");
            return new Hero(this);
        }
    }

    // свойства героя: читать их можно снаружи, а менять - только самому герою

    public string Code => HeroClass.Code;

    public string Name { get; }

    public HeroClass HeroClass { get; }

    public int Level { get; private set; }

    public int Experience { get; private set; }

    public int ExperienceToNextLevel => Level * ExperiencePerLevel;

    public int Gold { get; private set; }

    public int Health { get; private set; }

    public int MaxHealth => HeroClass.BaseHealth + (Level - 1) * HealthPerLevel;

    public bool IsAlive => Health > 0;

    public Weapon Weapon { get; private set; }

    public Armor Armor { get; private set; }

    public IReadOnlyList<Item> Inventory => inventory.AsReadOnly();

    public Hit RollDamage() {
        Hit hit = Weapon.RollDamage();
        return hit with { Damage = hit.Damage + (Level - 1) * DamagePerLevel };
    }

    public Damage ReceiveHit(int amount) {
        Damage damage = new(amount);
        defense.Handle(damage);
        Health = Math.Max(0, Health - damage.Amount);
        return damage;
    }

    // поведение героя между экспедициями

    // перед каждой экспедицией герой успевает отдохнуть
    public void Rest() {
        Health = MaxHealth;
        defense = HeroClass.CreateDefense(Armor, Level);
    }

    // начисляет опыт, возвращает количество полученных уровней
    public int GainExperience(int amount) {
        Experience += amount;
        int levelsGained = 0;
        while (Experience >= ExperienceToNextLevel) {
            Experience -= ExperienceToNextLevel;
            Level++;
            levelsGained++;
        }

        return levelsGained;
    }

    public void AddGold(int amount) {
        Gold += amount;
    }

    public void SpendGold(int amount) {
        if (amount > Gold) {
            throw new InvalidOperationException($"Не хватает золота: нужно {amount}, есть {Gold}");
        }

        Gold -= amount;
    }

    public void AddItem(Item item) {
        inventory.Add(item);
    }

    public void RemoveItem(Item item) {
        if (!inventory.Remove(item)) {
            throw new ArgumentException($"В инвентаре нет предмета {item.Name}");
        }
    }

    // экипирует предмет из инвентаря, снятый предмет возвращается в инвентарь
    public void Equip(Item item) {
        switch (item) {
        case Weapon newWeapon:
            RemoveItem(newWeapon);
            inventory.Add(Weapon);
            Weapon = newWeapon;
            break;
        case Armor newArmor:
            RemoveItem(newArmor);
            inventory.Add(Armor);
            Armor = newArmor;
            defense = HeroClass.CreateDefense(Armor, Level);
            break;
        default:
            throw new ArgumentException($"Предмет нельзя экипировать: {item.Name}");
        }
    }
    public bool Unlock(Achievement achievement) {
        if (achievements.Contains(achievement))
            return false;
        achievements.Add(achievement);
        return true;
    }
}