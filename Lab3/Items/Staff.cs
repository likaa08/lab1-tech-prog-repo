using Lab3.Combat;
using Lab3.Core;

namespace Lab3.Items;

// урон посоха непредсказуем: он может быть как больше, так и меньше базового
public sealed class Staff : Weapon
{
    private readonly double scatter;

    public Staff(string code, string name, int basePrice, int damage, double scatter)
        : base(code, name, basePrice, damage)
    {
        this.scatter = scatter;
    }

    private Staff(Staff source)
        : base(source)
    {
        scatter = source.scatter;
    }

    public override string Description => $"урон {MinDamage}-{MaxDamage}";

    private int MinDamage => (int)Math.Round(Damage * (1 - scatter), MidpointRounding.AwayFromZero);

    private int MaxDamage => (int)Math.Round(Damage * (1 + scatter), MidpointRounding.AwayFromZero);

    public override Hit RollDamage()
    {
        return new Hit(Dice.Instance.Roll(MinDamage, MaxDamage), false);
    }

    public override Staff Copy()
    {
        return new Staff(this);
    }
}