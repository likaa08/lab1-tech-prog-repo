using Lab3.Combat;

namespace Lab3.Items;

public abstract class Weapon : Item
{
    private readonly int damage;

    protected Weapon(string code, string name, int basePrice, int damage)
        : base(code, name, basePrice)
    {
        this.damage = damage;
    }

    protected Weapon(Weapon source)
        : base(source)
    {
        damage = source.damage;
    }

    // урон с учетом зачарований
    public int Damage => damage + Enchantments.Sum(enchantment => enchantment.BonusDamage);

    public override IReadOnlyList<Enchantment> PossibleEnchantments => Enchantment.ForWeapons;

    // как именно оружие наносит урон, решают наследники
    public abstract Hit RollDamage();

    public abstract override Weapon Copy();
}