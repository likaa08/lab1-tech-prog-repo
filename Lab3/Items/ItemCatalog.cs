using Lab3.Core;

namespace Lab3.Items;

// каталог хранит шаблоны предметов, а наружу отдает только их копии
public sealed class ItemCatalog
{
    // шаблоны хранятся в порядке добавления, предметов немного, поэтому ищем простым перебором
    private readonly List<Item> prototypes = new();

    public IReadOnlyList<string> Codes => prototypes.Select(prototype => prototype.Code).ToList();

    public void Register(Item prototype)
    {
        prototypes.Add(prototype);
    }

    // каталог не знает, какой конкретно класс у шаблона - копию создает сам шаблон
    public Item Create(string code)
    {
        Item? prototype = prototypes.FirstOrDefault(candidate => candidate.Code == code);
        if (prototype is null)
        {
            throw new UnknownCodeException("Предмет", code);
        }

        return prototype.Copy();
    }

    public Weapon CreateWeapon(string code)
    {
        return Create(code) as Weapon
               ?? throw new ArgumentException($"Предмет '{code}' не является оружием");
    }

    public Armor CreateArmor(string code)
    {
        return Create(code) as Armor
               ?? throw new ArgumentException($"Предмет '{code}' не является броней");
    }
}