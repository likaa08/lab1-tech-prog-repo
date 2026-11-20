using Lab3.Core;
using Lab3.Enemies;
using Lab3.Items;
namespace Lab3.World;
public sealed class HauntedManor : Location {
    public const string LocationCode = "haunted-manor";
    public HauntedManor(ItemCatalog catalog) : base(LocationCode, "Проклятый особняк", 2, catalog) {}
    protected override IReadOnlyList<string>
        LootTable => [DefaultCatalog.IronSword, DefaultCatalog.ElvenBow, DefaultCatalog.EmberStaff];
    protected override
        IEnemy CreateEnemy() => new HauntedWeapon(catalog.CreateWeapon(Dice.Instance.Pick(LootTable)));
}