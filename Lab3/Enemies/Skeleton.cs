namespace Lab3.Enemies;

// скелет: кости плохо поддаются ударам, поэтому часть урона проходит мимо
public sealed class Skeleton : AbstractEnemy
{
    private const double Resistance = 0.3;

    public Skeleton()
        : base("skeleton", "Скелет", 110, 10, 24, 90, 70)
    {
    }

    protected override int ReduceDamage(int damage)
    {
        return (int)Math.Round(damage * (1 - Resistance), MidpointRounding.AwayFromZero);
    }
}