namespace Lab3.Items;

public abstract class Item
{
    private readonly List<Enchantment> enchantments;

    protected Item(string code, string name, int basePrice)
    {
        Code = code;
        Name = name;
        BasePrice = basePrice;
        enchantments = new List<Enchantment>();
    }

    // конструктор копирования, его вызывают наследники в методе Copy()
    protected Item(Item source)
    {
        Code = source.Code;
        Name = source.Name;
        BasePrice = source.BasePrice;
        // список копируем, иначе копия и оригинал делили бы один и тот же объект списка зачарований
        enchantments = new List<Enchantment>(source.enchantments);
    }

    // код шаблона, из которого получен предмет
    public string Code { get; }

    public string Name { get; }

    private int BasePrice { get; }

    public int Price => BasePrice + enchantments.Sum(enchantment => enchantment.BonusPrice);

    public IReadOnlyList<Enchantment> Enchantments => enchantments.AsReadOnly();

    // какие зачарования подходят этому предмету
    public abstract IReadOnlyList<Enchantment> PossibleEnchantments { get; }

    // описание характеристик предмета, например "урон 12"
    public abstract string Description { get; }

    // каждый наследник по-своему реализует копирование
    public abstract Item Copy();

    public void Enchant(Enchantment enchantment)
    {
        if (!PossibleEnchantments.Contains(enchantment))
        {
            throw new ArgumentException($"Зачарование '{enchantment.Name}' не подходит для предмета '{Name}'");
        }

        enchantments.Add(enchantment);
    }
}