using Lab3.Combat;
using Lab3.Core;
using Lab3.Enemies;
using Lab3.Heroes;
using Lab3.Items;

namespace Lab3.World;

public abstract class Location
{
    private const double LootChance = 0.6;
    private const double EnchantChance = 0.3;

    private readonly ItemCatalog catalog;

    protected Location(string code, string displayName, int recommendedLevel, ItemCatalog catalog)
    {
        Code = code;
        DisplayName = displayName;
        RecommendedLevel = recommendedLevel;
        this.catalog = catalog;
    }

    public string Code { get; }

    public string DisplayName { get; }

    public int RecommendedLevel { get; }

    // коды предметов из каталога, которые можно найти в локации
    protected abstract IReadOnlyList<string> LootTable { get; }

    // создание врага делегируется подклассам для повышения гибкости
    protected abstract IEnemy CreateEnemy();

    public ExpeditionReport Explore(Hero hero)
    {
        return Explore(hero, Dice.Instance.NextSeed());
    }

    // бизнес-логика, общая для всех локаций:
    // герой встречает врага, сражается с ним и получает награду, если победил
    public ExpeditionReport Explore(Hero hero, int seed)
    {
        Dice dice = Dice.Instance;
        dice.Reseed(seed);

        hero.RestoreHealth();
        IEnemy enemy = CreateEnemy();
        BattleReport battle = new Battle(hero, enemy).Run();

        Rewards rewards = battle.Outcome == Outcome.Victory
            ? Reward(hero, enemy, dice)
            : Punish(hero);

        return new ExpeditionReport(seed, Code, battle, rewards);
    }

    private Rewards Reward(Hero hero, IEnemy enemy, Dice dice)
    {
        int experience = enemy.ExperienceReward;
        int gold = enemy.RollGoldReward();
        Item? loot = RollLoot(dice);

        int levelsGained = hero.GainExperience(experience);
        hero.AddGold(gold);
        if (loot is not null)
        {
            hero.AddItem(loot);
        }

        return new Rewards(experience, gold, loot, levelsGained);
    }

    private Item? RollLoot(Dice dice)
    {
        if (!dice.Chance(LootChance))
        {
            return null;
        }

        // добыча - это копия шаблона из каталога, ее можно менять, не боясь испортить шаблон
        Item loot = catalog.Create(dice.Pick(LootTable));
        if (dice.Chance(EnchantChance))
        {
            loot.Enchant(dice.Pick(loot.PossibleEnchantments));
        }

        return loot;
    }

    // проигравший герой теряет половину золота
    private static Rewards Punish(Hero hero)
    {
        int lostGold = hero.Gold / 2;
        hero.SpendGold(lostGold);
        return new Rewards(0, -lostGold, null, 0);
    }
}