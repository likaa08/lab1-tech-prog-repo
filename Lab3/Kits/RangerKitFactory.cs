using Lab3.Items;

namespace Lab3.Kits;

public sealed class RangerKitFactory : IStarterKitFactory
{
    private readonly ItemCatalog catalog;

    public RangerKitFactory(ItemCatalog catalog)
    {
        this.catalog = catalog;
    }

    public Weapon CreateWeapon()
    {
        return catalog.CreateWeapon(DefaultCatalog.HuntingBow);
    }

    public Armor CreateArmor()
    {
        return catalog.CreateArmor(DefaultCatalog.LeatherArmor);
    }
}