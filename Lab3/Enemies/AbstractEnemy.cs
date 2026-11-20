using Lab3.Defense;
using Lab3.Combat;
using Lab3.Core;

namespace Lab3.Enemies;

// общая логика для врагов: здоровье, урон в диапазоне, награда
public abstract class AbstractEnemy : IEnemy {
    private readonly int minDamage;
    private readonly int maxDamage;
    private readonly int goldReward;
    private readonly DamageHandler? defense;

    protected AbstractEnemy(string code, string name, int maxHealth, int minDamage, int maxDamage,
        int experienceReward, int goldReward, DamageHandler? defense = null) {
        Code = code;
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        ExperienceReward = experienceReward;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.goldReward = goldReward;
        this.defense = defense;
    }

    public string Code { get; }

    public string Name { get; }

    public int Health { get; private set; }

    public int MaxHealth { get; }

    public bool IsAlive => Health > 0;

    public int ExperienceReward { get; }

    // virtual - наследники могут переопределить этот метод
    public virtual Hit RollDamage() {
        return new Hit(Dice.Instance.Roll(minDamage, maxDamage), false);
    }

    public Damage ReceiveHit(int amount) {
        Damage damage = new(amount);
        defense?.Handle(damage);
        Health = Math.Max(0, Health - damage.Amount);
        return damage;
    }

    public int RollGoldReward() {
        return Dice.Instance.Roll(goldReward / 2, goldReward);
    }
}