using Lab3.Items;

namespace Lab3.Kits;

public sealed class MageKitFactory : IStarterKitFactory
{
    private readonly ItemCatalog catalog;

    public MageKitFactory(ItemCatalog catalog)
    {
        this.catalog = catalog;
    }

    public Weapon CreateWeapon()
    {
        return catalog.CreateWeapon(DefaultCatalog.ApprenticeStaff);
    }

    public Armor CreateArmor()
    {
        return catalog.CreateArmor(DefaultCatalog.LinenRobe);
    }
}