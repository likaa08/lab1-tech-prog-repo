using Lab3.Combat;
using Lab3.Core;

namespace Lab3.Items;

public sealed class Bow : Weapon
{
    private const int CriticalMultiplier = 2;

    private readonly double criticalChance;

    public Bow(string code, string name, int basePrice, int damage, double criticalChance)
        : base(code, name, basePrice, damage)
    {
        this.criticalChance = criticalChance;
    }

    private Bow(Bow source)
        : base(source)
    {
        criticalChance = source.criticalChance;
    }

    public override string Description => $"урон {Damage}, шанс крита {Math.Round(criticalChance * 100)}%";

    public override Hit RollDamage()
    {
        if (Dice.Instance.Chance(criticalChance))
        {
            return new Hit(Damage * CriticalMultiplier, true);
        }

        return new Hit(Damage, false);
    }

    public override Bow Copy()
    {
        return new Bow(this);
    }
}