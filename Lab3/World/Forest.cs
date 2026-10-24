using Lab3.Enemies;
using Lab3.Items;

namespace Lab3.World;

public sealed class Forest : Location
{
    public const string LocationCode = "forest";

    public Forest(ItemCatalog catalog)
        : base(LocationCode, "Мистический лес", 1, catalog)
    {
    }

    protected override IReadOnlyList<string> LootTable { get; } =
    [
        DefaultCatalog.GoblinCleaver, DefaultCatalog.HuntingBow, DefaultCatalog.LeatherArmor,
    ];

    protected override IEnemy CreateEnemy()
    {
        return new Goblin();
    }
}