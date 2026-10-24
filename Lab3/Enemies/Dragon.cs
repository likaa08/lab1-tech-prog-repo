using Lab3.Combat;
using Lab3.Core;

namespace Lab3.Enemies;

// дракон защищен чешуей и время от времени дышит огнем
public sealed class Dragon : AbstractEnemy
{
    private const double Resistance = 0.25;
    private const double FireBreathChance = 0.25;
    private const int FireBreathMultiplier = 2;

    public Dragon()
        : base("dragon", "Дракон", 260, 16, 22, 500, 400)
    {
    }

    public override Hit RollDamage()
    {
        Hit bite = base.RollDamage();
        if (Dice.Instance.Chance(FireBreathChance))
        {
            return new Hit(bite.Damage * FireBreathMultiplier, true);
        }

        return bite;
    }

    protected override int ReduceDamage(int damage)
    {
        return (int)Math.Round(damage * (1 - Resistance), MidpointRounding.AwayFromZero);
    }
}