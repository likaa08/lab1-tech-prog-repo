using Lab3.Core;
using Lab3.Heroes;
using Lab3.Items;

namespace Lab3.Camp;

public sealed class Merchant
{
    private const int SellPricePercent = 50;

    private readonly ItemCatalog catalog;
    private readonly List<string> assortment;

    public Merchant(ItemCatalog catalog, IEnumerable<string> assortment)
    {
        this.catalog = catalog;
        this.assortment = assortment.ToList();
    }

    // образцы товаров, чтобы показать их покупателю
    public IReadOnlyList<Item> Showcase => assortment.Select(catalog.Create).ToList();

    public Item Buy(Hero hero, string code)
    {
        if (!assortment.Contains(code))
        {
            throw new UnknownCodeException("Товар", code);
        }

        // покупатель получает собственную копию предмета из каталога
        Item item = catalog.Create(code);
        hero.SpendGold(item.Price);
        hero.AddItem(item);
        return item;
    }

    public int Sell(Hero hero, Item item)
    {
        int price = GetSellPrice(item);
        hero.RemoveItem(item);
        hero.AddGold(price);
        return price;
    }

    public int GetSellPrice(Item item)
    {
        return item.Price * SellPricePercent / 100;
    }
}