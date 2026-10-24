namespace Lab3.Enemies;

// гоблин - самый простой враг, без особенностей
public sealed class Goblin : AbstractEnemy
{
    public Goblin()
        : base("goblin", "Гоблин", 50, 6, 14, 40, 30)
    {
    }
}