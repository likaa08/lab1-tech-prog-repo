using Lab3.Enemies;
using Lab3.Items;

namespace Lab3.World;

public sealed class Crypt : Location
{
    public const string LocationCode = "crypt";

    public Crypt(ItemCatalog catalog)
        : base(LocationCode, "Заброшенный склеп", 3, catalog)
    {
    }

    protected override IReadOnlyList<string> LootTable { get; } =
    [
        DefaultCatalog.KnightBlade, DefaultCatalog.BoneBow, DefaultCatalog.NecromancerStaff, DefaultCatalog.CryptMail,
    ];

    protected override IEnemy CreateEnemy()
    {
        return new Skeleton();
    }
}