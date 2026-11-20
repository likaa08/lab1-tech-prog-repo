using Lab3.Heroes;
namespace Lab3.Storage;
public sealed class CachingHeroRepository(IHeroRepository target) : IHeroRepository {
    private Dictionary<string, Hero>? cache;
    private Dictionary<string, Hero> Cached() => cache ??= target.FindAll().ToDictionary(hero => hero.Name);
    public Hero? FindByName(string name) => Cached().GetValueOrDefault(name);
    public IReadOnlyList<Hero> FindAll() => Cached().Values.ToArray();
    public void Save(Hero hero) {
        target.Save(hero);
        Cached()[hero.Name] = hero;
    }
}