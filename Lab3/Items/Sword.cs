using Lab3.Combat;

namespace Lab3.Items;

public sealed class Sword : Weapon
{
    public Sword(string code, string name, int basePrice, int damage)
        : base(code, name, basePrice, damage)
    {
    }

    private Sword(Sword source)
        : base(source)
    {
    }

    public override string Description => $"урон {Damage}";

    public override Hit RollDamage()
    {
        return new Hit(Damage, false);
    }

    public override Sword Copy()
    {
        return new Sword(this);
    }
}