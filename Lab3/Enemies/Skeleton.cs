using Lab3.Defense;
namespace Lab3.Enemies;

// скелет: кости плохо поддаются ударам, поэтому часть урона проходит мимо
public sealed class Skeleton : AbstractEnemy {
    private const double Resistance = 0.3;

    public Skeleton() : base("skeleton", "Скелет", 110, 10, 24, 90, 70, new ArmorHandler(Resistance)) {}
}