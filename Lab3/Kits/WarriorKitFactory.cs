using Lab3.Items;

namespace Lab3.Kits;

public sealed class WarriorKitFactory : IStarterKitFactory
{
    private readonly ItemCatalog catalog;

    public WarriorKitFactory(ItemCatalog catalog)
    {
        this.catalog = catalog;
    }

    public Weapon CreateWeapon()
    {
        return catalog.CreateWeapon(DefaultCatalog.TrainingSword);
    }

    public Armor CreateArmor()
    {
        return catalog.CreateArmor(DefaultCatalog.Chainmail);
    }
}