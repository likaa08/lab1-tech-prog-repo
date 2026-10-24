using Lab3.Items;

namespace Lab3.Kits;

// Абстрактная фабрика: стартовый набор снаряжения
public interface IStarterKitFactory
{
    // возвращает некое оружие
    Weapon CreateWeapon();

    // возвращает некую броню
    Armor CreateArmor();
}