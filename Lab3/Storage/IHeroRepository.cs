using Lab3.Heroes;
namespace Lab3.Storage;
public interface IHeroRepository {
    Hero? FindByName(string name);
    IReadOnlyList<Hero> FindAll();
    void Save(Hero hero);
}