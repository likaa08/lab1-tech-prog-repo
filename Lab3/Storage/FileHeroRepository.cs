using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using Lab3.Heroes;
using Lab3.Items;
using Lab3.Progress;
namespace Lab3.Storage;
public sealed class FileHeroRepository(string file, ItemCatalog catalog) : IHeroRepository {
    private static readonly JsonSerializerOptions Options =
        new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
    public Hero? FindByName(string name) => FindAll().FirstOrDefault(hero => hero.Name == name);
    public IReadOnlyList<Hero> FindAll() {
        if (!File.Exists(file))
            return [];
        string text = File.ReadAllText(file, Encoding.UTF8);
        if (string.IsNullOrWhiteSpace(text))
            return [];
        var saves = JsonSerializer.Deserialize<List<HeroSave>>(text, Options) ??
                    throw new InvalidDataException("В файле должен быть массив героев");
        return saves.Select(Restore).ToArray();
    }
    public void Save(Hero hero) {
        var saves = FindAll().Where(other => other.Name != hero.Name).Select(Snapshot).ToList();
        saves.Add(Snapshot(hero));
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(file))!);
        File.WriteAllText(file, JsonSerializer.Serialize(saves, Options) + "\n", new UTF8Encoding(false));
    }
    private static ItemSave SnapshotItem(Item item) => new(item.Code,
                                                           item.Enchantments.Select(e => e.Code).ToList());
    private static HeroSave Snapshot(Hero hero) => new(hero.Name, hero.HeroClass.Code, hero.Level,
                                                       hero.Experience, hero.Gold, SnapshotItem(hero.Weapon),
                                                       SnapshotItem(hero.Armor),
                                                       hero.Inventory.Select(SnapshotItem).ToList(),
                                                       hero.Achievements.Select(a => a.Code).ToList());
    private Item RestoreItem(ItemSave save) {
        Item item = catalog.Create(save.Code);
        foreach (string code in save.Enchantments)
            item.Enchant(Enchantment.ByCode(code));
        return item;
    }
    private Hero Restore(HeroSave save) => new Hero.Builder()
                                               .SetName(save.Name)
                                               .SetHeroClass(HeroClass.ByCode(save.HeroClass))
                                               .SetLevel(save.Level)
                                               .SetExperience(save.Experience)
                                               .SetGold(save.Gold)
                                               .SetWeapon((Weapon)RestoreItem(save.Weapon))
                                               .SetArmor((Armor)RestoreItem(save.Armor))
                                               .SetInventory(save.Inventory.Select(RestoreItem))
                                               .SetAchievements(save.Achievements.Select(Achievement.ByCode))
                                               .Build();
    private sealed record ItemSave(string Code, List<string> Enchantments);
    private sealed record HeroSave(string Name, string HeroClass, int Level, int Experience, int Gold,
                                   ItemSave Weapon, ItemSave Armor, List<ItemSave> Inventory,
                                   List<string> Achievements);
}