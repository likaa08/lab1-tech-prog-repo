using Lab3.Combat;
using Lab3.Core;

namespace Lab3.Enemies;

// общая логика для врагов: здоровье, урон в диапазоне, награда
public abstract class AbstractEnemy : IEnemy
{
    private readonly int minDamage;
    private readonly int maxDamage;
    private readonly int goldReward;

    protected AbstractEnemy(
        string code, string name, int maxHealth,
        int minDamage, int maxDamage,
        int experienceReward, int goldReward)
    {
        Code = code;
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        ExperienceReward = experienceReward;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.goldReward = goldReward;
    }

    public string Code { get; }

    public string Name { get; }

    public int Health { get; private set; }

    public int MaxHealth { get; }

    public bool IsAlive => Health > 0;

    public int ExperienceReward { get; }

    // virtual - наследники могут переопределить этот метод
    public virtual Hit RollDamage()
    {
        return new Hit(Dice.Instance.Roll(minDamage, maxDamage), false);
    }

    public int ReceiveHit(int damage)
    {
        int dealt = Math.Max(1, ReduceDamage(damage));
        Health = Math.Max(0, Health - dealt);
        return dealt;
    }

    public int RollGoldReward()
    {
        return Dice.Instance.Roll(goldReward / 2, goldReward);
    }

    // наследники могут уменьшать входящий урон
    protected virtual int ReduceDamage(int damage)
    {
        return damage;
    }
}