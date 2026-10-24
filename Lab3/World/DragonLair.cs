using Lab3.Enemies;
using Lab3.Items;

namespace Lab3.World;

public sealed class DragonLair : Location
{
    public const string LocationCode = "dragon-lair";

    public DragonLair(ItemCatalog catalog)
        : base(LocationCode, "Логово дракона", 5, catalog)
    {
    }

    protected override IReadOnlyList<string> LootTable { get; } =
    [
        DefaultCatalog.DawnBlade, DefaultCatalog.DragonscaleArmor,
    ];

    protected override IEnemy CreateEnemy()
    {
        return new Dragon();
    }
}